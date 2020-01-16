import { Action, Reducer } from "redux";
import { AppThunkAction } from "./";
import { JwtTokenPayload, JwtManager } from "../server/JwtManager/JwtManager";
import { LoginClient } from "../server/LoginClient";
export type ClearSession_Action = ClearSessionAction;

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface SessionState {
  username: string;
  permissions: string[];
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface SetSessionAction {
  type: "SET_SESSION";
  token: JwtTokenPayload | undefined;
}

interface ClearSessionAction {
  type: "CLEAR_SESSION";
}

interface RenewTokenAction {
  type: "RENEW_TOKEN";
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = SetSessionAction | ClearSessionAction | RenewTokenAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
  setSession: (token: JwtTokenPayload): AppThunkAction<KnownAction> => (
    dispatch,
    getState
  ) => {
    dispatch({ type: "SET_SESSION", token: token });
  },
  clearSession: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
    dispatch({ type: "CLEAR_SESSION" });
  },
  renewToken: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
    const appState = getState();
    if (appState && appState.session) {
      const client = new LoginClient();
      client
        .renewToken()
        .then(token => {
          let jwt: JwtManager = new JwtManager(null);
          let payload: JwtTokenPayload = jwt.save(token);
          dispatch({ type: "SET_SESSION", token: payload });
        })
        .catch(data => {
          dispatch({ type: "CLEAR_SESSION" });
        });
    }
    dispatch({ type: "RENEW_TOKEN" });
  }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: SessionState = { username: "", permissions: [] };

export const reducer: Reducer<SessionState> = (
  state: SessionState | undefined,
  incomingAction: Action
): SessionState => {
  if (state === undefined) {
    return unloadedState;
  }

  const action = incomingAction as KnownAction;
  switch (action.type) {
    case "SET_SESSION":
      return {
        username: action.token ? action.token.username : "",
        permissions: action.token ? [" ", ...action.token.permissions] : []
      };
    case "CLEAR_SESSION":
      return {
        username: "",
        permissions: []
      };
  }

  return state;
};
