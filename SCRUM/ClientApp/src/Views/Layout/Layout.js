import React from 'react';
import { Layout as AntDLayout, Image } from 'antd';
import "antd/dist/antd.css";

import logo from "./logo.PNG";
import NavMenu from './NavMenu';
import "./Layout.css";

const { Header, Footer } = AntDLayout;

const Layout = (props) => {
  return (
    <AntDLayout className="layout">
      <Header>
        <div className="logo" >
          <Image src={logo} preview={false} />
        </div>
        <NavMenu />
      </Header>
      <div className="site-layout-content">
        {props.children}
      </div>
      <Footer style={{ textAlign: "center" }}>
        ©2021 System Zarządzania Wymaganiami Projektów SCRUM
      </Footer>
    </AntDLayout>
  )
};

export default Layout;
