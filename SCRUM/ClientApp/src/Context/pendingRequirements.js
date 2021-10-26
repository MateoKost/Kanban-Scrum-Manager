import React, { useEffect, useState, useContext } from "react";

import { ProjectsContext } from "./projects";
import PendingApiClient from "../API/PendingApiClient";

export const ProposalsContext = React.createContext();

export const ProposalsProvider = ({ children }) => {

  const [proposals, setProposals] = useState({ data: [], status: "pending" });

  const { oid } = useContext(ProjectsContext);

  useEffect(() => {
    if(oid!=null && oid!="")
      ReadMyPendingRequirements();
  }, [oid]);


  useEffect(() => {
    if(oid!=null && oid!="")
      ReadMyPendingRequirements();
  }, []);



  const ReadMyPendingRequirements = async () => {
    await PendingApiClient.ReadMyPendingRequirements({oid}).then((result) => {
      setProposals({ data: result.data, status: "fulfilled" });
    });
  }

  const CreateNewPendingRequirement = async ( params ) => {
    params = { ProjectId: Number(oid), ...params};
    console.log(params);
    await PendingApiClient.CreateNewPendingRequirement( params ).then((result) => {
      ReadMyPendingRequirements();
    });
  }

  const RemovePendingRequirement = async ({ pendingOid }) => {
    await PendingApiClient.RemovePendingRequirement(oid, pendingOid).then((result) => {
      ReadMyPendingRequirements();
    });
  }


  const EditPendingRequirement = async ({ payload }) => {
    payload = { ProjectId: Number(oid), ...payload};
    await PendingApiClient.EditPendingRequirement( { payload  }).then((result) => {
      ReadMyPendingRequirements();
    });
  }

  return (
    <ProposalsContext.Provider
      value={{
        CreateNewPendingRequirement,
        ReadMyPendingRequirements,
        EditPendingRequirement,
        RemovePendingRequirement,
        proposals,
      }}
    >
      {children}
    </ProposalsContext.Provider>
  );
};
