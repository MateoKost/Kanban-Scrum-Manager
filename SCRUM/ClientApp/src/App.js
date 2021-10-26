import React from 'react';
import { Route } from 'react-router';
import {withCookies} from 'react-cookie';

import Layout from "./Views/Layout/Layout";
import Home from './Views/Home/Home';
import Projects from './Views/Projects/Projects';
import EditablePendingTable from './Views/PendingRequirements/EditablePendingTable';
import Kanban from './Views/Kanban/Kanban';
import './custom.css'

import { ProposalsProvider } from './Context/pendingRequirements';
import { RequirementsProvider } from './Context/requirements';
import { AuthProvider } from './Context/authorization';
import { ProjectsProvider } from './Context/projects';

function App() {
  return (
    <AuthProvider>
      <ProjectsProvider>
      <Layout>
        {/* <Route exact path='/' component={Home} /> */}
        <ProposalsProvider>
          <Route exact path='/projects' component={Projects} />
          <RequirementsProvider>
          <Route path='/pendings' component={EditablePendingTable} />
          <Route path='/kanban' component={Kanban} />
          </RequirementsProvider>
        </ProposalsProvider>
      </Layout>
      </ProjectsProvider>
    </AuthProvider>
  );
}

export default withCookies(App);

