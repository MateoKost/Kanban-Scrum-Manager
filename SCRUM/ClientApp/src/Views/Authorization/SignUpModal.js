import React, { useContext, useState } from 'react';
import { Modal, Button, Form, notification, Input, } from 'antd';
import { UserOutlined, LockOutlined, SmileOutlined } from '@ant-design/icons';
import 'antd/dist/antd.css';

import { AuthContext } from '../../Context/authorization';

const SignUpModal = () => {

  const {initialize} = useContext(AuthContext);
  const [loading, setLoading] = useState(false);
  const [visible, setVisible] = useState(false);
  const [form] = Form.useForm();

  const showModal = () => {
    setVisible(true);
  };

  const handleOk = async () => {
    handleSumbit(null);
  };

  const handleCancel = () => {
    if (!loading)
      setVisible(false);
      setVisible(false);
  };

  const openNotification = () => {
    notification.open({
      message: 'Pomyślnie zarejestrowano użytkownika',
      description:
        'Użytkownik uwierzytelniony',
      icon: <SmileOutlined style={{ color: '#108ee9' }} />,
    });
  };


  const handleSumbit = async (values) => {

    initialize().then(() => {
        setLoading(false);
        setVisible(false);
        // form.resetFields();
        // openNotification();
      });;

    // let params = {
    //   Login: values.login,
    //   //Password: values.password,
    // };

    // signIn({ payload: params}).then(()=>openNotification());
  }

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

  // let content;
  // content =
  //   <Form
  //     form={form}
  //     layout="horizontal"
  //     onFinish={handleSumbit}
  //     {...formItemLayout}
  //   >
  //     <Form.Item label="Login" name="login" rules={[{ required: true, message: 'Login jest wymagany!' }]}>
  //       <Input prefix={<UserOutlined className="site-form-item-icon" />} />
  //     </Form.Item>
  //     <Form.Item label="Hasło" name="password" rules={[{ required: false, message: 'Hasło jest wymagane!' }]}>
  //       <Input type="password" prefix={<LockOutlined className="site-form-item-icon" />} />
  //     </Form.Item>
  //   </Form>


  return (
    <>
     <a onClick={showModal}> Zarejestruj się </a>
      {/* <Button type="primary" onClick={showModal}>
        Dołącz
      </Button> */}
      <Modal
        // title="Zarejestruj się do systemu"
        title="Dane testowe"
        visible={visible}
        onOk={handleOk}
        onCancel={handleCancel}
        footer={[
          <Button key="back" onClick={handleCancel} >
            Anuluj
          </Button>,
          <Button key="submit" type="primary" loading={loading} onClick={handleOk}>
            Dodaj
          </Button>,
        ]}
      >
        <p>Przycisk <b>"Dodaj"</b> zainicjuje dane przykladowe.</p>
        <p>Zostanie utworzony użytkownik o nazwie: "StakeHolder". Nastąpi przypisanie jednego projektu z dwoma wymaganiami.</p>
        {/* {content} */}
      </Modal>
    </>
  );
}


export default SignUpModal;