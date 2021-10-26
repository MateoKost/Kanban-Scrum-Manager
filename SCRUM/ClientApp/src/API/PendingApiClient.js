import { APIclient } from "./APIclient";

const ReadMyPendingRequirements = async ( {oid} ) => {
    return await APIclient.get({ endpoint: `/api/pendingrequirements/${oid}`, body: {}, customConfig : {} });
}

const CreateNewPendingRequirement = async ( payload ) => {
    return await APIclient.post({ endpoint: `/api/pendingrequirements`, body: payload, customConfig : {} });
}

const RemovePendingRequirement = async ( projectOid, pendingOid ) => {
    const endpoint = `/api/pendingrequirements/${projectOid}/${pendingOid}`;
    return await APIclient.delete({ endpoint, body: {}, customConfig : {} });    
}
    
const EditPendingRequirement = async ( {payload} ) => {
    const endpoint = `/api/pendingrequirements`;
    return await APIclient.put({ endpoint, body: payload, customConfig : {} });    
}


export default { ReadMyPendingRequirements, CreateNewPendingRequirement, RemovePendingRequirement, EditPendingRequirement };  