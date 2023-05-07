import React from "react";
import { Layout } from "./Layout";
import { AppState, AppStateContext } from "./AppContext";
import { configure } from "mobx";

const appState = new AppState();

configure({
    enforceActions: "always",
});

export const App = () => {
    return (
        <AppStateContext.Provider value={appState}>
            <Layout />
        </AppStateContext.Provider>
    );
};
