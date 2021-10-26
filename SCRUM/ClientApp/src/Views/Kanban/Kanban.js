import React, { useContext } from 'react';
import { Col, Spin, PageHeader, Row, Layout, Space, Card, Divider, Breadcrumb, message, } from 'antd';
import { CheckOutlined, HourglassOutlined } from '@ant-design/icons';
import 'antd/dist/antd.css';
import "./Dragable.css";
import { useHistory } from "react-router-dom";
import SortableTable from './DnD';
import { RequirementsContext } from '../../Context/requirements';
import { ProjectsContext } from '../../Context/projects';

const { Content } = Layout;

const Kanban = () => {

  const { requirements, UpdateRequirementOrder } = useContext(RequirementsContext);
  const { project } = useContext(ProjectsContext);

  let history = useHistory();

  let acceptedRequirements;
  let inWorkRequirements;
  let accomplishedRequirements;
  let canceledRequirements;

  if (requirements.status === "pending") {
    console.log("pending");
  } else if (requirements.status === "fulfilled") {
    console.log("fulfilled");
    console.log(requirements.data);
    acceptedRequirements = requirements.data.filter((r) => r.status === "Zaakceptowane").sort((a, b) => a.index - b.index);
    inWorkRequirements = requirements.data.filter((r) => r.status === "W trakcie").sort((a, b) => a.index - b.index);
    accomplishedRequirements = requirements.data.filter((r) => r.status === "Zakończone").sort((a, b) => a.index - b.index);
    canceledRequirements = requirements.data.filter((r) => r.status === "Anulowane").sort((a, b) => a.index - b.index);
  } else if (requirements.status === "failed") {
  }

  const newBigOrder = async (data, category) => {

    let maxIndex = 0;
    let params;

    category === "Zaakceptowane" ?
      params = data.map((req, i) => ({ ProjectId: req.projectId, Oid: req.oid, Index: maxIndex + i })) :
      params = acceptedRequirements.map((req, i) => ({ ProjectId: req.projectId, Oid: req.oid, Index: maxIndex + i }))

    maxIndex = params.length;
    category === "W toku" ?
      params = params.concat(data.map((req, i) => ({ ProjectId: req.projectId, Oid: req.oid, Index: maxIndex + i }))):
      params = params.concat(inWorkRequirements.map((req, i) => ({ ProjectId: req.projectId, Oid: req.oid, Index: maxIndex + i })))

    maxIndex = params.length;
    category === "Zakończone" ?
      params = params.concat(data.map((req, i) => ({ ProjectId: req.projectId, Oid: req.oid, Index: maxIndex + i }))):
      params = params.concat(accomplishedRequirements.map((req, i) => ({ ProjectId: req.projectId, Oid: req.oid, Index: maxIndex + i })))

    maxIndex = params.length;
    category === "Anulowane" ?
      params = params.concat(data.map((req, i) => ({ ProjectId: req.projectId, Oid: req.oid, Index: maxIndex + i }))):
      params = params.concat(canceledRequirements.map((req, i) => ({ ProjectId: req.projectId, Oid: req.oid, Index: maxIndex + i })))

    UpdateRequirementOrder(params).then(() => {});
  }

  const breadcrumb = (
    <Breadcrumb>
      <Breadcrumb.Item><a href="/projects">Projekty</a></Breadcrumb.Item>
      <Breadcrumb.Item>{project ? project.title : ""}</Breadcrumb.Item>
      <Breadcrumb.Item>Realizacja wymagań</Breadcrumb.Item>
    </Breadcrumb>
  );


  let content =
    <>
      <Row gutter={16}>
        <Col className="gutter-row" span={8} >
          <Card className="card-no-space" title="Zaakceptowane" type="inner" style={{ height: "100%" }} >
            {acceptedRequirements && < SortableTable contentData={acceptedRequirements} contentCategory="Zaakceptowane" newBigOrder={newBigOrder} />}
          </Card>
        </Col>
        <Col className="gutter-row" span={8} >
          <Card className="card-no-space" title="W toku" type="inner" style={{ height: "100%" }} extra={[<HourglassOutlined />]}>
            {inWorkRequirements && < SortableTable contentData={inWorkRequirements} contentCategory="W toku" newBigOrder={newBigOrder} />}
          </Card>
        </Col>
        <Col className="gutter-row" span={8} >
          <Card className="card-no-space" title="Zakończone" type="inner" style={{ height: "100%" }} extra={[<CheckOutlined />]}>
            {accomplishedRequirements && < SortableTable contentData={accomplishedRequirements} contentCategory="Zakończone" newBigOrder={newBigOrder} />}
          </Card>
        </Col>
      </Row>
      <Divider />
      <Row gutter={16}>
        <Col className="gutter-row" span={8}>
          <Card className="card-no-space" title="Anulowane" type="inner"  >
            {canceledRequirements && < SortableTable contentData={canceledRequirements} contentCategory="Anulowane" newBigOrder={newBigOrder} />}
          </Card>
        </Col>
      </Row>
    </>

  return (
    <PageHeader
      title="Przebieg projektu"
      className="site-page-header"
      onBack={() => history.push("/projects")}
      extra={[
      ]}
      breadcrumb={breadcrumb}
    >
      <Content
        extraContent={
          <img
            src="https://gw.alipayobjects.com/zos/antfincdn/K%24NnlsB%26hz/pageHeader.svg"
            alt="content"
            width="100%"
          />
        }
      >
        {content}
      </Content>
    </PageHeader>
  );
}

export default Kanban;