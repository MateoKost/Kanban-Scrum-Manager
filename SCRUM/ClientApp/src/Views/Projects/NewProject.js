import React, { useContext, useState } from 'react';
import { Modal, Button, Form, notification, Input } from 'antd';
import { SmileOutlined } from '@ant-design/icons';
import 'antd/dist/antd.css';

import { ProjectsContext } from '../../Context/projects';

const CreateModal = () => {

  const {CreateProject} = useContext(ProjectsContext);

  const [loading, setLoading] = useState(false);
  const [visible, setVisible] = useState(false);
  const [form] = Form.useForm();
  const { TextArea } = Input;

  const showModal = () => {
    setVisible(true);
  };

  const handleSumbit = async (values) => {
    let params = {
      Title: values.title,
      Description: values.description,
      Tag: values.tag,
    };
    CreateProject({ payload: params }).then(() => {
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
      message: 'Pomyślnie utworzono projekt',
      description:
        'Wybierz nowo utworzony projekt, aby zobaczyć wymagania.',
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
    layout="horizontal"
    onFinish={handleSumbit}
    {...formItemLayout}
  >
    <Form.Item label="Tytuł" name="title" rules={[{ required: true, message: 'Tytuł projektu jest wymagany!' }]}>
      <Input />
    </Form.Item>
    <Form.Item label="Tag" name="tag" rules={[{ required: true, message: 'Tag projektu jest wymagany!' }]}>
      <Input />
    </Form.Item>
    <Form.Item label="Opis" disabled = {()=>true} name="description" rules={[{ required: true, message: 'Opis projektu jest wymagany!' }]}>
      <TextArea />
    </Form.Item>
  </Form>

  return (
    <>
      <Button type="primary" onClick={showModal}>
        Dodaj
      </Button>
      <Modal
        title="Nowy projekt"
        visible={visible}
        onOk={handleOk}
        onCancel={handleCancel}
        footer={[
          <Button key="back" onClick={handleCancel} >
            Anuluj
          </Button>,
          <Button key="submit" type="primary" loading={loading} onClick={handleOk}>
            Utwórz
          </Button>,
        ]}
      >
        {content}
      </Modal>
    </>
  );
}


export default CreateModal;