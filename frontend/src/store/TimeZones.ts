import { Action, Reducer } from "redux";
import { AppThunkAction } from ".";
import { TimeZoneDto, TimeZoneClient } from "../server/TimeZoneClient";

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface TimeZonesState {
  isLoading: boolean;
  timezones: TimeZoneDto[];
  errorMessage: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestTimeZonesAction {
  type: "REQUEST_TIMEZONES";
}

interface ReceiveTimeZonesAction {
  type: "RECEIVE_TIMEZONES";
  timezones: TimeZoneDto[];
}

interface DeleteTimeZoneAction {
  type: "DELETE_TIMEZONE";
  key: string;
}

interface CreateTimeZoneAction {
  type: "CREATE_TIMEZONE";
  timezone: TimeZoneDto;
}

interface UpdateTimeZoneAction {
  type: "UPDATE_TIMEZONE";
  timezone: TimeZoneDto;
}

interface SetErrorAction {
  type: "TZ_SET_ERROR";
  errorMessage: string;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction =
  | RequestTimeZonesAction
  | ReceiveTimeZonesAction
  | DeleteTimeZoneAction
  | CreateTimeZoneAction
  | UpdateTimeZoneAction
  | SetErrorAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
  requestTimeZones: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
    const appState = getState();
    const client = new TimeZoneClient();
    if (appState && appState.timezones) {
      client
        .load("")
        .then(data => {
          dispatch({ type: "RECEIVE_TIMEZONES", timezones: data });
        })
        .catch(dispatchErrorMessageAction(dispatch));

      dispatch({ type: "REQUEST_TIMEZONES" });
    }
  },
  deleteTimeZone: (key: string): AppThunkAction<KnownAction> => dispatch => {
    const client = new TimeZoneClient();
    client
      .delete(key)
      .then(_ => {
        dispatch({ type: "DELETE_TIMEZONE", key: key });
      })
      .catch(dispatchErrorMessageAction(dispatch));
  },
  createTimeZone: (
    timezone: TimeZoneDto
  ): AppThunkAction<KnownAction> => dispatch => {
    const client = new TimeZoneClient();
    client
      .create(timezone)
      .then(createdTimeZone => {
        dispatch({ type: "CREATE_TIMEZONE", timezone: createdTimeZone });
      })
      .catch(dispatchErrorMessageAction(dispatch));
  },
  updateTimeZone: (
    timezone: TimeZoneDto
  ): AppThunkAction<KnownAction> => dispatch => {
    const client = new TimeZoneClient();
    client
      .update(timezone)
      .then(updatedTimeZone => {
        dispatch({ type: "UPDATE_TIMEZONE", timezone: updatedTimeZone });
      })
      .catch(dispatchErrorMessageAction(dispatch));
  }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: TimeZonesState = {
  timezones: [],
  isLoading: false,
  errorMessage: ""
};

export const reducer: Reducer<TimeZonesState> = (
  state: TimeZonesState | undefined,
  incomingAction: Action
): TimeZonesState => {
  if (state === undefined) {
    return unloadedState;
  }

  const action = incomingAction as KnownAction;
  switch (action.type) {
    case "REQUEST_TIMEZONES":
      return {
        timezones: state.timezones,
        isLoading: true,
        errorMessage: ""
      };
    case "RECEIVE_TIMEZONES":
      return {
        timezones: action.timezones,
        isLoading: false,
        errorMessage: ""
      };
    case "DELETE_TIMEZONE":
      return {
        timezones: state.timezones.filter(tz => tz.key !== action.key),
        isLoading: false,
        errorMessage: ""
      };
    case "CREATE_TIMEZONE":
      return {
        timezones: [action.timezone, ...state.timezones],
        isLoading: false,
        errorMessage: ""
      };
    case "UPDATE_TIMEZONE":
      return {
        timezones: [
          action.timezone,
          ...state.timezones.filter(tz => tz.key !== action.timezone.key)
        ],
        isLoading: false,
        errorMessage: ""
      };
    case "TZ_SET_ERROR":
      return {
        timezones: state.timezones,
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
      type: "TZ_SET_ERROR",
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
