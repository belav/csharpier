import { AppContext, useSetupAppContext } from "AppContext";
import React from "react";
import { Layout } from "./Layout";

export const App = () => {
    return (
        <AppContext.Provider value={useSetupAppContext()}>
            <Layout />
        </AppContext.Provider>
    );
};
