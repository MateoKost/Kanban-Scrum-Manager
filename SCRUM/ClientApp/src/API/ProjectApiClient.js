import { APIclient } from "./APIclient";

const ReadAllProjects = async () => {
    return await APIclient.get({ endpoint: '/api/projects' });
}


const CreateProject = async (payload) => {
    return await APIclient.post({ endpoint: '/api/projects', body: payload, customConfig : {} });
}

export default { ReadAllProjects, CreateProject };

// const CreateProject = async ({ payload }) => {
//     const endpoint = serverURL + "/api/projects";
//     const method = "POST";
//     // ProjectId
//     await APIclient(endpoint, method, { body: payload }).then((result) => {
//       ReadAllProjects({ payload: null });
//     });
//   }
