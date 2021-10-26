import React, { useState, useContext } from 'react';
import { useHistory } from "react-router-dom";
import {
  Table, InputNumber, Popconfirm, Form, Button, Spin, notification,  Breadcrumb, Tooltip,
  PageHeader, Layout, message
} from 'antd';
import { EditOutlined, CheckOutlined, CloseOutlined, ArrowRightOutlined, QuestionOutlined } from '@ant-design/icons';
import "./Editable.css";

import { ProposalsContext } from '../../Context/pendingRequirements';
import { ProjectsContext } from '../../Context/projects';
import ProposalRemover from './PendingRemover';
import CreateModal from './NewPending';
import NewRequirementModal from './NewRequirement';
import TextArea from 'rc-textarea';

const { Content} = Layout;

const error = () => {
  message.error('This is an error message');
};

const openNotification = () => {
  notification.open({
    message: 'Pomyślnie zmieniono zgłoszenie',
    description:
      'Zgłoszenie zostanie zweryfikowane przez właściciela projektu.',
    icon: <EditOutlined style={{ color: '#108ee9' }} />,
  });
};

const EditableCell = ({
  editing,
  dataIndex,
  title,
  inputType,
  record,
  index,
  children,
  ...restProps
}) => {
  const inputNode = inputType === 'number' ? <InputNumber /> : <TextArea autoSize={{ minRows: 6 }} style={{
    margin: 0,
    width: "100%",
  }} />;
  return (
    <td {...restProps}>
      {editing ? (
        <Form.Item
          size="large" autoSize={{ minRows: 6 }}
          name={dataIndex}
          style={{
            margin: 0,
            width: "100%",
          }}
          rules={[
            {
              required: true,
              message: `Please Input ${title}!`,
            },
          ]}
        >
          {inputNode}
        </Form.Item>
      ) : (
        children
      )}
    </td>
  );
};

const EditablePendingTable = () => {

  const { project, projectPermissions } = useContext(ProjectsContext);
  const { proposals, EditPendingRequirement, ReadProtectedRequirements } = useContext(ProposalsContext);

  let history = useHistory();
  const [form] = Form.useForm();
  const [data, setData] = useState([]);
  const [editingKey, setEditingKey] = useState('');
  const [visible, setVisible] = useState(false);
  const [pendingRecord, setPendingRecord] = useState(null);

  const isEditing = (record) => record.oid === editingKey;
  const cancel = () => { setEditingKey(''); };

  const accept = async (record) => {
    setPendingRecord(record);
    setVisible(true);
  };

  const edit = (record) => {
    form.setFieldsValue({
      title: '',
      description: '',
      ...record,
    });
    setEditingKey(record.oid);
  };

  const save = async (oid) => {
    try {
      const row = await form.validateFields();
      const newData = [...data];
      const index = newData.findIndex((item) => oid === item.oid);

      let params = {
        Oid: oid,
        Title: row.title,
        Description: row.description,
      };
      EditPendingRequirement({ payload: params }).then((result) => {
        openNotification();
        setEditingKey('');
      });
    } catch (errInfo) {
      console.log('Validate Failed:', errInfo);
    }
  };

  const columns = [
    {
      title: 'Status',
      width: "5%",
      align: "center",
      editable: false,
      render: (_, record) => {
        return record.status === "Oczekuje" ? (
          <Tooltip title="Oczekuje">
            <QuestionOutlined />
          </Tooltip>
        ) : (
          <Tooltip title="Odrzucone">
            <CloseOutlined />
          </Tooltip>
        )
      }
    },
    {
      title: 'Tytuł',
      dataIndex: 'title',
      key: 'title',
      editable: true,
    },
    {
      title: 'Opis',
      dataIndex: 'description',
      key: 'description',
      editable: true,
      render: (_, record) => {
        return (
          <pre>{record.description}</pre>
        )
      }
    },
    {
      title: 'Akcje',
      dataIndex: '',
      key: 'x',
      width: 150,
      render: (_, record) => {
        const editable = isEditing(record);
        return editable ? (
          <div>
            <Tooltip title="Zapisz">
              <Button size="small" style={{
                marginRight: 8,
              }} type="primary" shape="circle" icon={<CheckOutlined />} onClick={() => save(record.oid)} />
            </Tooltip>
            <Popconfirm title="Czy anulować zmiany?" okText="Tak" cancelText="Nie" onConfirm={cancel} >
              <Tooltip title="Odrzuć">
                <Button size="small" style={{
                  marginRight: 8,
                }} primary shape="circle" icon={<CloseOutlined />} />
              </Tooltip>
            </Popconfirm>
          </div>
        ) : (
          <div> {
            projectPermissions && projectPermissions.includes("AcceptProposal") && 
            <Tooltip title="Zaakceptuj">
              <Button size="small" style={{
                marginRight: 8,
              }} type="primary" shape="circle" icon={<ArrowRightOutlined />} onClick={() => accept(record)} />
            </Tooltip>
              }
               {
            projectPermissions.includes("UpdateMyProposal") && 
            <Tooltip title="Edytuj">
              <Button size="small" style={{
                marginRight: 8,
              }} primary shape="circle" icon={<EditOutlined />} onClick={() => edit(record)} />
            </Tooltip>
                 }
            {
            record.status === "Oczekuje" && projectPermissions && projectPermissions.includes("DeclineProposal") &&
              <ProposalRemover record={record} />
            }
          </div>
        )
      }
    }
  ];

  const mergedColumns = columns.map((col) => {
    if (!col.editable) {
      return col;
    }

    return {
      ...col,
      onCell: (record) => ({
        record,
        inputType: col.dataIndex === 'title' ? 'description' : 'text',
        dataIndex: col.dataIndex,
        title: col.title,
        editing: isEditing(record),
      }),
    };
  });

  let content;
  if (proposals.status === "pending") {
    console.log("pending");
    content = <Spin size="large" />;
  } else if (proposals.status === "fulfilled") {
    console.log("fulfilled");

    let pendings = proposals.data && proposals.data.filter((p) => p.status === "Oczekuje").sort((a, b) => a.oid - b.oid);
    let declined = proposals.data && proposals.data.filter((p) => p.status === "Odrzucone").sort((a, b) => a.oid - b.oid);

    content = proposals.data &&
      <div centered>
        <Form form={form} component={false}   >
          <Table
            components={{
              body: {
                cell: EditableCell,
              },
            }}
            bordered
            dataSource={[...pendings, ...declined]} columns={columns}
            columns={mergedColumns}
            rowClassName="editable-row"
            pagination={{
              onChange: cancel,
            }}
          />
        </Form>
      </div>
  } else if (proposals.status === "failed") {
    content = <div>error!!!</div>;
  }

  const breadcrumb = (
    <Breadcrumb>
      <Breadcrumb.Item><a href="/projects">Projekty</a></Breadcrumb.Item>
      <Breadcrumb.Item>{project ? project.title : ""}</Breadcrumb.Item>
      <Breadcrumb.Item>Proponowane wymagania</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageHeader
      title="Propozycje wymagań"
      className="site-page-header"
      // subTitle={project ? project.title : ""}
      onBack={() => history.push("/projects")}
      extra={[ 
        projectPermissions && projectPermissions.includes("CreateProposal") && <CreateModal />,
    ]}
      breadcrumb={breadcrumb}
    >
      <Content>
        <NewRequirementModal visible={visible} setVisible={setVisible} pendingRecord={pendingRecord} />
        {content}
      </Content>
    </PageHeader>

  );
};

export default EditablePendingTable;
