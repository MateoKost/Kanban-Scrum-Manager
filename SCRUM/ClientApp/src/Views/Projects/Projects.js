import React, {useContext} from 'react';
import { useHistory } from "react-router-dom";
import { Table, Button, Spin, PageHeader, Layout, Space, Breadcrumb } from 'antd';
import { ProjectsContext } from '../../Context/projects';
import CreateModal from './NewProject';

const {Content} = Layout;

const Projects = () => {

  let history = useHistory();

  const { projects, oid, setOid } = useContext(ProjectsContext);

  const selectProject = (project) => {
    history.push({
      pathname: '/pendings',
      search: '?project='+project.oid,
    });
  }

  const kanbanProject = (project) => {
    history.push({
      pathname: '/kanban',
      search: '?project='+project.oid,
    });
  }

  const breadcrumb = (
    <Breadcrumb>
      <Breadcrumb.Item>Projekty</Breadcrumb.Item>
    </Breadcrumb>
  );

  const columns = [
    {
      title: 'Tytuł',
      dataIndex: 'title',
      key: 'title',
    },
    {
      title: 'Opis',
      dataIndex: 'description',
      key: 'description',
    },
    {
      title: 'Tag',
      dataIndex: 'tag',
      key: 'tag',
    },
    {
      title: 'Akcje',
      key: 'action',
      width: 200,
      render: (text, record) => (
        ( oid && oid === record.oid ) ? ( "wybrany" ) : (
          <Space direction = "vertical" align="center">
        <Button style={{minWidth: "170px"}} type="primary" onClick={()=>selectProject(record)}>Propozycje wymagań</Button> 
        <Button style={{minWidth: "170px"}} type="primary" onClick={()=>kanbanProject(record)}>Widok Kanban</Button> 
        </Space>
        )
      ),
    },
  ];
  
  let content;
  if (projects.status === "pending") {
    console.log("pending");
    content = <Spin size="large" />;
  } else if (projects.status === "fulfilled") {
    console.log("fulfilled");
    content = <Table  columns={columns} dataSource={projects.data.sort((a, b) => a.oid - b.oid)}/>
  } else if (projects.status === "failed") {
    content = <div>error!!!</div>;
  }

  return (
    <PageHeader
    title="Projekty"
    className="site-page-header"
    extra={[<CreateModal/>,]}
    breadcrumb={ breadcrumb }
  >
    <Content>
      {content}
    </Content>
  </PageHeader>
  );
}

export default Projects;