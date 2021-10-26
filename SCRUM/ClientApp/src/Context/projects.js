import React, { useEffect, useState, useContext, useCallback } from "react";
import { useLocation } from "react-router-dom";

import ProjectApiClient from "../API/ProjectApiClient";
import { AuthContext } from "./authorization";

export const ProjectsContext = React.createContext();

export const ProjectsProvider = ({ children }) => {

  const { signOut, userName, permissions } = useContext(AuthContext);

  const location = useLocation();
  const [projects, setProjects] = useState({ data: [], status: "pending" });
  const [oid, setOid] = useState([]);
  const [project, setProject] = useState([]);
  const [projectPermissions, setProjectPermissions] = useState([]);

  useEffect(() => {
    if(userName !== null )
      ReadAllProjects();
    else {
      setOid([]);
      setProject([]);
      setProjectPermissions([]);
    }
  }, [userName]);

  useEffect( () => {
    const projectQuery = "?project=";
      if(location.search.includes(projectQuery)){
        setOid(location.search.split(projectQuery)[1]);
      }
   }, [location]);

  useEffect(  () => { 
    if(projects.status==="fulfilled" && oid){
      const searchProject = projects.data && projects.data.find( p => p.oid == oid);
      if(searchProject) {
        setProject(searchProject);
        let filteredPermissions = permissions.filter( (p) => p.projectId == oid ).map((p)=>(p.permission));
        console.log(filteredPermissions);
        setProjectPermissions(filteredPermissions);
      }
    }
  }, [oid, projects.status]);


  const ReadAllProjects = async () => {
    await ProjectApiClient.ReadAllProjects().then((result) => {
      setProjects({ data: result.data, status: "fulfilled" });
    });
  }

  const CreateProject = async ({ payload }) => {
    await ProjectApiClient.CreateProject(payload).then((result) => {
      ReadAllProjects();
    });
  }

  return (
    <ProjectsContext.Provider
      value={{
        oid,
        setOid,
        projects,
        project,
        projectPermissions,
        CreateProject,
      }}
    >
      {children}
    </ProjectsContext.Provider>
  );
};
