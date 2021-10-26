import React, { useEffect, useState } from "react";
import { useHistory } from "react-router-dom";

import { APIclient } from "../API/APIclient";
import UserApiClient from "../API/UserApiClient";

export const AuthContext = React.createContext();

export const AuthProvider = ({ children }) => {
  
  const [userName, setUserName] = useState(null);
  const [permissions, setPermissions] = useState(null);

  let history = useHistory();

  useEffect(() => {
    getMyProfile();
    getMyPermissions();
  }, []);

  const getMyProfile = async () => {
    await UserApiClient.getMyProfile().then((result) => {
      setUserName(result.data.name);
    });
  }

  const getMyPermissions = async () => {
    await UserApiClient.getMyPermissions().then((result) => {
      setPermissions(result.data);
    });
  }


  const signIn = async ( params ) => {

    await UserApiClient.signIn(params).then((result) => {
        getMyProfile();
        getMyPermissions();
        history.push("/projects");
      });
  }


  const initialize = async () => {
    const endpoint = "/api/initialize";
    const method = "GET";
    await APIclient(endpoint, method).then((result) => {
    });
  }


  const signOut = async () => {
    await UserApiClient.signOut().then(() => {
      setUserName(null);
      history.push("/");
    });
  }

  return (
    <AuthContext.Provider
      value={{
        signIn,
        signOut,
        permissions,
        userName
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

