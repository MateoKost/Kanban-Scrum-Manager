import { APIclient } from "./APIclient";

const signIn = async (userName) => {
    return await APIclient.post(  { endpoint: '/api/signIn', body: userName ,  customConfig : {} } );
}

const getMyProfile = async () => {
    return await APIclient.get(  { endpoint: '/api/getMyProfile' } );
}

const getMyPermissions = async () => {
    return await APIclient.get(  { endpoint: '/api/getMyPermissions' } );
}

const signOut = async () => {
    return await APIclient.get(  { endpoint: '/api/signout' } );
}


export default {signIn, getMyProfile, getMyPermissions, signOut} ;