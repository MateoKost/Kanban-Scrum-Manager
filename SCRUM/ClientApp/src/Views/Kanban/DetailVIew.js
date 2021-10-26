import React, { useContext, useState, useEffect } from 'react';
import { Modal, Button, Form, notification, Input, InputNumber, } from 'antd';
import { SmileOutlined } from '@ant-design/icons';
import 'antd/dist/antd.css';

import { RequirementsContext } from '../../Context/requirements';
import { ProjectsContext } from '../../Context/projects';

const DetailView = ({ pendingRecord, visible, setVisible }) => {

  const { EditAcceptedRequirement,EditStartedRequirement, } = useContext(RequirementsContext);
  const { projectPermissions } = useContext(ProjectsContext);
  const [loading, setLoading] = useState(false);
  const [form] = Form.useForm();
  const { TextArea } = Input;

  useEffect(() => {
    form.setFieldsValue({
      ...pendingRecord,
    });
  }, [pendingRecord]);


  const handleSumbit = async (values) => {

    let params = {
      Oid: pendingRecord.oid,
      Title: values.title,
      Description: values.description,
      Touchstone: values.touchstone,
      Priority: values.priority,
      Effortfulness: values.effortfulness,
    };

    if (pendingRecord.status === "Zaakceptowane") {
      EditAcceptedRequirement(params).then(() => {
        setLoading(false);
        setVisible(false);
        form.resetFields();
        openNotification();
      });
    } else if (pendingRecord.status === "W trakcie") {
      EditStartedRequirement(params).then(() => {
        setLoading(false);
        setVisible(false);
        form.resetFields();
        openNotification();
      });
    }
  }

  const handleOk = async () => {
    form.validateFields().then(values => {
      setLoading(true);
      handleSumbit(values);
    })
      .catch(info => {
        console.log('Validate Failed:', info);
      });
  };

  const handleCancel = () => {
    if (!loading)
      setVisible(false);
  };

  const openNotification = () => {
    notification.open({
      message: 'Zmodyfikowano wymaganie',
      description:
        'Wymaganie zostało zmodyfikowano.',
      icon: <SmileOutlined style={{ color: '#108ee9' }} />,
    });
  };


  let editAllowed =
    pendingRecord &&
      projectPermissions && pendingRecord.status === "Zaakceptowane" ?
      (projectPermissions && projectPermissions.includes("UpdateNotStartedRequirement") && true)
      :
      (projectPermissions && projectPermissions.includes("UpdateRequirement") && true)


  let content = <Form
    form={form}
    layout="vertical"
    onFinish={handleOk}

  >
    {
      pendingRecord && <>
        <Form.Item label="Tytuł" name="title" rules={[{ required: true, message: 'Tytuł jest wymagany!' }]}>
          <TextArea size="large" autoSize={{ minRows: 2 }} />
        </Form.Item>
        <Form.Item label="Treść" name="description" rules={[{ required: true, message: 'Opis jest wymagany!' }]}>
          <TextArea size="large" autoSize={{ minRows: 6 }} />
        </Form.Item>
        <Form.Item label="Kryterium akceptacyjne" name="touchstone" rules={[{ required: true, message: 'Określ kryterium akceptacyjne!' }]}>
          <Input />
        </Form.Item>
        <Input.Group compact >
          <Form.Item style={{ marginRight: "10px", width: "50%" }} label="Priorytet" name="priority" rules={[{ required: true, message: 'Określ priorytet!' }]}>
            <InputNumber style={{ width: '100%' }} min={1} max={10} />
          </Form.Item>
          <Form.Item style={{ width: 'calc(50% - 10px)' }} label="Szacowana pracochłonność" name="effortfulness" rules={[{ required: true, message: 'Oszacuj pracochłonność!' }]}>
            <InputNumber style={{ width: '100%' }} min={1} max={10} />
          </Form.Item>
        </Input.Group>
      </>
    }
  </Form>

  return (
    <>
      <Modal
        title="Szczegóły wymagania"
        visible={visible}
        onOk={handleOk}
        centered
        closable={false}
        style={{ marginTop: "10px" }}
        width={"90%"}
        footer={[
          <Button key="back" onClick={handleCancel} >
            Anuluj
          </Button>,
          editAllowed &&
          <Button key="submit" type="primary" loading={loading} onClick={handleOk}>
            Wprowadź zmiany i zamknij
          </Button>,
        ]}
      >
        {content}
      </Modal>
    </>
  );
}


export default DetailView;