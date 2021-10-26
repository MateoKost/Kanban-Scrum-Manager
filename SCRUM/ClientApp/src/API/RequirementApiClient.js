import { APIclient } from "./APIclient";


const ReadAllRequirements = async ( {oid} ) => {
    return await APIclient.get({ endpoint: `/api/requirements/${oid}` });
}


const CreateNewRequirement = async (payload) => {
    return await APIclient.post({ endpoint: `/api/requirements/${payload.PendingId}`, body: payload, customConfig : {} });
}


const EditAcceptedRequirement = async (payload) => {
    return await APIclient.put({ endpoint: `/api/requirements`, body: payload, customConfig : {} });
}


const EditStartedRequirement = async (payload) => {
    return await APIclient.put({ endpoint: `/api/requirements/started`, body: payload, customConfig : {} });
}


const UpdateRequirementStatus = async (payload) => {
    return await APIclient.put({ endpoint: `/api/requirements/status`, body: payload, customConfig : {} });
}

//send array of new order
const UpdateRequirementOrder = async (payload) => {
    return await APIclient.put({ endpoint: `/api/requirements/order`, body: payload, customConfig : {} });
}


export default { ReadAllRequirements, CreateNewRequirement, UpdateRequirementStatus, EditAcceptedRequirement, EditStartedRequirement, UpdateRequirementOrder };







// const CreateNewRequirement = async ({ payload }) => {
//     const endpoint = serverURL + "/requirements/" + payload.PendingId;
//     const method = "POST";
//     payload = { ProjectId: Number(oid),
//                 Status: "To do",
//                 ...payload};
//     await APIclient(endpoint, method, { body: payload }).then((result) => {
//       ReadAllRequirements({ payload: null });
//       ReadMyPendingRequirements({ payload: null });
//       ReadAllPendingRequirements({ payload: null });
//     });
//   }

//   const ReadAllRequirements = async () => {
//     const endpoint = serverURL + "/requirements/" + oid;
//     const method = "GET";
//     await APIclient(endpoint, method).then((result) => {
//       setRequirements({ data: result.data, status: "fulfilled" });
//     });
//   }