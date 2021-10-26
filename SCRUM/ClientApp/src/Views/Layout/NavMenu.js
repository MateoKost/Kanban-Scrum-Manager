import React, { useContext } from 'react';
import { Link } from 'react-router-dom';
import { Menu } from "antd";

import SignInModal from '../Authorization/SignInModal';
import SignUpModal from '../Authorization/SignUpModal';
import "./NavMenu.css";

import { AuthContext } from '../../Context/authorization';

const NavMenu = () => {

  const { signOut, userName } = useContext(AuthContext);

  let content = userName ? (
    <>
      <Menu.Item style={{ float: "right" }} key={6} onClick={signOut}>
        Wyloguj
      </Menu.Item>
      <Menu.Item style={{ float: "right", overflow: "hidden" }} key={3} className="modified-item" >
        Zalogowany jako  {userName}
      </Menu.Item>
    </>
  ) : (
    <Menu.Item style={{ float: "right" }} key={0}>
      <SignInModal /></Menu.Item>)

  return (
    <Menu theme="dark" mode="horizontal" defaultSelectedKeys={["1"]}>
      <Menu.Item key={1}><Link to="/projects" />Projekty</Menu.Item>
      { content }
      {/* <Menu.Item style={{ float: "right" }} key={5}>
        <SignUpModal /></Menu.Item> */}
    </Menu>
  );
}

export default NavMenu;