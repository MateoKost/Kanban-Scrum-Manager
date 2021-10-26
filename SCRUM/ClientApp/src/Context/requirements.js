import React, { useEffect, useState, useContext } from "react";

import { ProjectsContext } from "./projects";
import { ProposalsContext } from "./pendingRequirements";
import RequirementApiClient from "../API/RequirementApiClient";

export const RequirementsContext = React.createContext();

export const RequirementsProvider = ({ children }) => {

  const [requirements, setRequirements] = useState({ data: [], status: "pending" });
  const { oid } = useContext(ProjectsContext);
  const { ReadMyPendingRequirements } = useContext(ProposalsContext);

  useEffect(() => {
    if(oid!=null && oid!="")
      ReadAllRequirements();
  }, [oid]);

  useEffect(() => {
    if(oid!=null && oid!=""){
      ReadAllRequirements();
    }
  }, []);


  const ReadAllRequirements = async () => {
    await RequirementApiClient.ReadAllRequirements({oid}).then((result) => {
      console.log("reqs");
      console.log(result.data);
      setRequirements({ data: result.data, status: "fulfilled" });
    });
  }


  const CreateNewRequirement = async ( payload ) => {
    payload = {
      ProjectId: Number(oid),
      Status: "To do",
      ...payload
    };
    await RequirementApiClient.CreateNewRequirement( payload ).then((result) => {
      ReadMyPendingRequirements();
      ReadAllRequirements();
    });
  }

  const UpdateRequirementStatus = async ( payload ) => {
    payload = {
      ProjectId: Number(oid),
      ...payload
    };
    await RequirementApiClient.UpdateRequirementStatus( payload ).then((result) => {
      ReadAllRequirements();
    });
  }


  const EditAcceptedRequirement = async ( payload ) => {
    payload = {
      ProjectId: Number(oid),
      ...payload
    };
    await RequirementApiClient.EditAcceptedRequirement( payload ).then((result) => {
      ReadAllRequirements();
    });
  }


  const EditStartedRequirement = async ( payload ) => {
    payload = {
      ProjectId: Number(oid),
      ...payload
    };

    await RequirementApiClient.EditStartedRequirement( payload ).then((result) => {
      ReadAllRequirements();
    });
  }

  const UpdateRequirementOrder = async ( payload ) => {
    await RequirementApiClient.UpdateRequirementOrder( payload ).then((result) => {
      ReadAllRequirements();
    });
  }

  return (
    <RequirementsContext.Provider
      value={{
        requirements,
        CreateNewRequirement,
        UpdateRequirementStatus,
        EditAcceptedRequirement, 
        EditStartedRequirement, 
        UpdateRequirementOrder
      }}
    >
      {children}
    </RequirementsContext.Provider>
  );
};
