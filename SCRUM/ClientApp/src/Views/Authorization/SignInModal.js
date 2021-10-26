import React, { useContext, useState } from 'react';
import { Modal, Button, Form, notification, Input, Divider, Radio} from 'antd';
import { UserOutlined, LockOutlined, SmileOutlined } from '@ant-design/icons';
import 'antd/dist/antd.css';

import { AuthContext } from '../../Context/authorization';

const SignInModal = () => {

  const { signIn } = useContext(AuthContext);
  const [loading, setLoading] = useState(false);
  const [visible, setVisible] = useState(false);
  const [value, setValue] = useState(1);
  const [form] = Form.useForm();

  const onChange = e => {
    console.log('radio checked', e.target.value);
    setValue(e.target.value);
  };

  const showModal = () => { setVisible(true); };

  const handleOk = async () => {

    const userName = value === 1 ? ("StakeHolder") : (value === 2 ? ("ProductOwner") : ("DevTeam"));
    setLoading(true);

    let params = {
      Name: userName,
    };

    signIn( params ).then(() => {
      setLoading(false);
      setVisible(false);
      form.resetFields();
      openNotification();
    }
    );
  };

  const handleCancel = () => {
    if (!loading)
      setVisible(false);
    setVisible(false);
  };

  const openNotification = () => {
    notification.open({
      message: 'Pomyślnie zalogowano',
      description:
        'Użytkownik uwierzytelniony',
      icon: <SmileOutlined style={{ color: '#108ee9' }} />,
    });
  };


  // const handleSumbit = async (userName) => {

  //   let params = {
  //     Name: userName,
  //     // Login: values.login,
  //     //Password: values.password,
  //   };

  //   // console.log(params);

  //   signIn( params ).then(() => {
  //     setLoading(false);
  //     setVisible(false);
  //     form.resetFields();
  //     openNotification();
  //   }
  //   );
  // }

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
  content =
    <Form
      form={form}
      layout="horizontal"
      // onFinish={handleSumbit}
      {...formItemLayout}
    >

      <Radio.Group centered style={{ display: 'inline-flex', justifyContent: 'center', alignItems: 'center' }}
        onChange={onChange}
        value={value}
      >
        <Radio value={1}>Interesariusz</Radio>
        <Radio value={2}>Product owner</Radio>
        <Radio value={3}>Developer</Radio>
      </Radio.Group>

      {/* <Form.Item label="Login" name="login" rules={[{ required: true, message: 'Login jest wymagany!' }]}>
        <Input prefix={<UserOutlined className="site-form-item-icon" />} />
      </Form.Item>
      <Form.Item label="Hasło" name="password" rules={[{ required: false, message: 'Hasło jest wymagane!' }]}>
        <Input type="password" prefix={<LockOutlined className="site-form-item-icon" />} />
      </Form.Item> */}
    </Form>


  return (
    <>
      <a
         ghost 
        onClick={showModal}>
        {/* <SearchOutlined />  */}
        Zaloguj
      </a>
      {/* <Button ghost onClick={showModal}>
        Zaloguj
      </Button> */}
      <Modal
        title="Zaloguj się do swojego konta"
        visible={visible}
        onOk={handleOk}
        onCancel={handleCancel}
        footer={[
          <Button key="back" onClick={handleCancel} >
            Anuluj
          </Button>,
          <Button key="submit" type="primary" loading={loading} onClick={handleOk}>
            Zaloguj się
          </Button>,
        ]}
      >
        {content}
      </Modal>
    </>
  );
}


export default SignInModal;