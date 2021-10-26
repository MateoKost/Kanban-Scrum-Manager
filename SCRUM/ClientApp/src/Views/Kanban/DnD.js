import React, { useState, useContext, useEffect } from 'react';
import { Table, Card, Button, notification, Space } from 'antd';
import { MenuOutlined, ArrowRightOutlined, SmileOutlined, ArrowLeftOutlined, DeleteOutlined, CloseOutlined, EditOutlined, EyeOutlined } from '@ant-design/icons';

import { sortableContainer, sortableElement, sortableHandle } from 'react-sortable-hoc';
import { arrayMoveImmutable } from 'array-move';
import "./Dragable.css";

import { RequirementsContext } from '../../Context/requirements';
import { ProjectsContext } from '../../Context/projects';

import DetailView from './DetailVIew';

export const SortableTable = ({ contentData, contentCategory, newBigOrder }) => {

  const { projectPermissions } = useContext(ProjectsContext);
  const [dataSource, setDataSource] = useState(contentData);

  const DragHandle = sortableHandle(() => <MenuOutlined style={{ cursor: 'grab', color: '#999' }} />);

  const nextStatus = (currentStatus, direction) => {
    if ((currentStatus === "Zaakceptowane" || currentStatus === "W trakcie") && direction === "down") {
      return "Anulowane";
    }
    if (currentStatus === "Zaakceptowane" || currentStatus === "Zakończone") {
      return "W trakcie";
    } else if (currentStatus === "W trakcie" && direction === "right") {
      return "Zakończone";
    } else if (currentStatus === "W trakcie" && direction === "left") {
      return "Zaakceptowane";
    }
  }

  const changeReqStatus = async (record, direction) => {
    let newStatus = nextStatus(record.status, direction);
    let params = {
      Oid: record.oid,
      Status: newStatus,
    };
    UpdateRequirementStatus(params).then(() => {
      openNotification();
    });
  };

  const { UpdateRequirementStatus, UpdateRequirementOrder } = useContext(RequirementsContext);

  const [visible, setVisible] = useState(false);
  const [pendingRecord, setPendingRecord] = useState(null);


  const accept = async (record) => {
    setPendingRecord(record);
    setVisible(true);
  };


  const openNotification = () => {
    notification.open({
      message: 'Zmieniono kolejność',
      description:
        'Zmieniono kolejność wymagania.',
      icon: <SmileOutlined style={{ color: '#108ee9' }} />,
    });
  };


  const updateOrder = async (data) => {
    newBigOrder(data, contentCategory).then(() => {
      openNotification();
    });
  }



  useEffect(() => {
    setDataSource(contentData);
  }, [contentData]);


  const columns = [
    {
      title: 'Sort',
      dataIndex: 'sort',
      width: 30,
      className: 'drag-visible',
      render: () => <DragHandle />,

    },
    {
      key: 'action',
      className: 'drag-visible',
      render: (text, record) => (
        <Card size="small" className="card-no-space card-no-space-2nd" style={{ marginRight: "10px" }}
          //  title={ <a style={{    wordBreak: "break-all"}} class="underline-on-hover" onClick={() =>accept(record) }>{record.title}</a>}
          actions={projectPermissions && projectPermissions.includes("ChangeRequirementStatusAndTime") && [
            "Priorytet: " + record.priority,
            [
              ["W trakcie"].includes(record.status)
              &&
              <Button
                onClick={() => changeReqStatus(record, "left")}
                size="small"
                primary shape="circle"
                icon={<ArrowLeftOutlined />} />
              ,
              ["Zaakceptowane", "W trakcie"].includes(record.status)
              &&
              <Button onClick={() => changeReqStatus(record, "down")}
                size="small"
                primary shape="circle"
                icon={<CloseOutlined />} />
              ,
              ["Zaakceptowane", "W trakcie"].includes(record.status)
              &&
              <Button
                onClick={() => changeReqStatus(record, "right")}
                size="small"
                primary shape="circle"
                icon={<ArrowRightOutlined />} />
            ]
          ]}
        >
          <a style={{ wordBreak: "break-all", marginRight: "10px" }} class="underline-on-hover"
            onClick={() => accept(record)}
          >{record.title}</a>
        </Card>
      ),
    },
  ];

  const SortableItem = sortableElement(props => <tr {...props} />);
  const SortableContainer = sortableContainer(props => <tbody {...props} />);



  const onSortEnd = ({ oldIndex, newIndex }) => {
    if (oldIndex !== newIndex) {
      const newData = arrayMoveImmutable([].concat(dataSource), oldIndex, newIndex).filter(el => !!el);
      console.log('Sorted items: ', newData);
      setDataSource(newData);
      updateOrder(newData);
    }
  };

  const DraggableContainer = (props) => (
    <SortableContainer
      useDragHandle
      disableAutoscroll
      helperClass="row-dragging"
      onSortEnd={projectPermissions && projectPermissions.includes("ReorderRequirements") && onSortEnd}
      {...props}
    />
  );

  const DraggableBodyRow = ({ className, style, ...restProps }) => {
    const index = dataSource.findIndex(x => x.index === restProps['data-row-key']);
    return <SortableItem index={index} {...restProps} />;
  };


  return (
    <>
      <Table
        showHeader={false}
        pagination={false}
        dataSource={dataSource}
        columns={columns}
        rowKey="index"
        components={{
          body: {
            wrapper: DraggableContainer,
            row: DraggableBodyRow,
          },
        }}
      />
      <DetailView pendingRecord={pendingRecord} visible={visible} setVisible={setVisible} />
    </>
  );
}


export default SortableTable;

