import React, { useContext, useState } from 'react';
import { Modal, Button, Form, notification, Input } from 'antd';
import { SmileOutlined } from '@ant-design/icons';
import 'antd/dist/antd.css';

import { ProposalsContext } from '../../Context/pendingRequirements';

const CreateModal = () => {

  const { CreateNewPendingRequirement } = useContext(ProposalsContext);

  const [loading, setLoading] = useState(false);
  const [visible, setVisible] = useState(false);

  const [form] = Form.useForm();
  const { TextArea } = Input;

  const showModal = () => setVisible(true);

  const handleSumbit = async (values) => {
    let params = {
      Title: values.title,
      Description: values.description,
    };
    CreateNewPendingRequirement( params ).then(() => {
      setLoading(false);
      setVisible(false);
      form.resetFields();
      openNotification();
    });
  }

  const handleOk = async () => {
    form.validateFields().then(values => {
        setLoading(true);
        setTimeout(() => {
          handleSumbit(values);
        }, 500);
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
      message: 'Pomyślnie wysłano zgłoszenie',
      description:
        'Zgłoszenie zostanie zweryfikowane przez właściciela projektu.',
      icon: <SmileOutlined style={{ color: '#108ee9' }} />,
    });
  };

  const formItemLayout = {
    labelCol: {
      xs: { span: 4 },
      sm: { span: 4 },
    },
    wrapperCol: {
      xs: { span: 28 },
      sm: { span: 28 },
    },
  };

  let content;
  content = <Form
    form={form}
    layout="vertical"
    onFinish={handleSumbit}
    {...formItemLayout}
  >

    <Form.Item label="Tytuł" name="title" rules={[{ required: true, message: 'Tytuł zgłoszenia jest wymagany!' }]}>
    <TextArea size="large" autoSize={{minRows: 2}} />
    </Form.Item>
    <Form.Item label="Treść" disabled = {()=>true} name="description" rules={[{ required: true, message: 'Opis zgłoszenia jest wymagany!' }]}>
      <TextArea size="large" autoSize={{minRows: 6}} />
    </Form.Item>

  </Form>

  return (
    <>
      <Button type="primary" onClick={showModal}>
        Dodaj
      </Button>
      <Modal
        title="Nowa propozycja wymagania"
        visible={visible}
        onOk={handleOk}
        onCancel={handleCancel}
        footer={[
          <Button key="back" onClick={handleCancel} >
            Anuluj
          </Button>,
          <Button key="submit" type="primary" loading={loading} onClick={handleOk}>
            Wyślij
          </Button>,
        ]}
      >
        {content}
      </Modal>
    </>
  );
}

export default CreateModal;