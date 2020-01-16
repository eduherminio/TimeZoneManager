import { Action, Reducer } from "redux";
import { AppThunkAction } from "./";
import { UserDto, UserClient, FullUserDto } from "../server/UserClient";

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface UsersState {
  isLoading: boolean;
  users: UserDto[];
  errorMessage: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestUsersAction {
  type: "REQUEST_USERS";
}

interface ReceiveUsersAction {
  type: "RECEIVE_USERS";
  users: UserDto[];
}

interface DeleteUserAction {
  type: "DELETE_USER";
  key: string;
}

interface CreateUserAction {
  type: "CREATE_USER";
  user: UserDto;
}

interface UpdateUserAction {
  type: "UPDATE_USER";
  user: UserDto;
}

interface SetErrorAction {
  type: "USER_SET_ERROR";
  errorMessage: string;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction =
  | RequestUsersAction
  | ReceiveUsersAction
  | DeleteUserAction
  | CreateUserAction
  | UpdateUserAction
  | SetErrorAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
  requestUsers: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
    const appState = getState();
    const client = new UserClient();
    if (appState && appState.users) {
      client
        .load("")
        .then(data => {
          dispatch({ type: "RECEIVE_USERS", users: data });
        })
        .catch(dispatchErrorMessageAction(dispatch));

      dispatch({ type: "REQUEST_USERS" });
    }
  },
  deleteUser: (key: string): AppThunkAction<KnownAction> => dispatch => {
    const client = new UserClient();
    client
      .delete(key)
      .then(_ => {
        dispatch({ type: "DELETE_USER", key: key });
      })
      .catch(dispatchErrorMessageAction(dispatch));
  },
  createUser: (user: FullUserDto): AppThunkAction<KnownAction> => dispatch => {
    const client = new UserClient();
    client
      .create(user)
      .then(createdUser => {
        dispatch({ type: "CREATE_USER", user: createdUser });
      })
      .catch(dispatchErrorMessageAction(dispatch));
  },
  updateUser: (user: UserDto): AppThunkAction<KnownAction> => dispatch => {
    const client = new UserClient();
    client
      .update(user)
      .then(updatedUser => {
        dispatch({ type: "UPDATE_USER", user: updatedUser });
      })
      .catch(dispatchErrorMessageAction(dispatch));
  }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: UsersState = {
  users: [],
  isLoading: false,
  errorMessage: ""
};

export const reducer: Reducer<UsersState> = (
  state: UsersState | undefined,
  incomingAction: Action
): UsersState => {
  if (state === undefined) {
    return unloadedState;
  }

  const action = incomingAction as KnownAction;
  switch (action.type) {
    case "REQUEST_USERS":
      return {
        users: state.users,
        isLoading: true,
        errorMessage: ""
      };
    case "RECEIVE_USERS":
      return {
        users: action.users,
        isLoading: false,
        errorMessage: ""
      };
    case "DELETE_USER":
      return {
        users: state.users.filter(tz => tz.key !== action.key),
        isLoading: false,
        errorMessage: ""
      };
    case "CREATE_USER":
      return {
        users: [action.user, ...state.users],
        isLoading: false,
        errorMessage: ""
      };
    case "UPDATE_USER":
      return {
        users: [
          action.user,
          ...state.users.filter(tz => tz.key !== action.user.key)
        ],
        isLoading: false,
        errorMessage: ""
      };
    case "USER_SET_ERROR":
      return {
        users: state.users,
        isLoading: false,
        errorMessage: action.errorMessage
      };
  }

  return state;
};

function dispatchErrorMessageAction(
  dispatch: (action: KnownAction) => void
): ((reason: any) => void | PromiseLike<void>) | null | undefined {
  return error => {
    dispatch({
      type: "USER_SET_ERROR",
      errorMessage:
        error.response && typeof JSON.parse(error.response) === "string"
          ? JSON.parse(error.response)
          : error.response
          ? error.response
          : error.message
          ? error.message
          : error.toString()
    });
  };
}
