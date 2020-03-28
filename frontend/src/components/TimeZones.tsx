/**
 * Source of tableRef workaround for detecting Edit/Delete state:
 * https://stackoverflow.com/questions/58144937/check-if-a-material-table-row-is-still-in-edit-mode
*/

import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as TimeZonesStore from '../store/TimeZones';
import { TimeZoneDto } from '../server/TimeZoneClient';
import * as moment from 'moment';
import { Button } from '../formcontrols/Button/Button';
import tableIcons from '../formcontrols/tableIcons';
import MaterialTable from 'material-table';
import { CircularProgress } from '@material-ui/core';
import './FormStyles.css';

type rowType = { key: string | undefined; name: string; cityName: string; gmtDiff: number; timeThere: string; localDiff: number; ownerUsername: string | undefined; };

// At runtime, Redux will merge together...
type TimeZonesProps =
  TimeZonesStore.TimeZonesState // ... state we've requested from the Redux store
  & typeof TimeZonesStore.actionCreators // ... plus action creators we've requested
  & RouteComponentProps<{}>; // ... plus incoming routing parameters

interface TimeZonesState {
  tableref: any,
  aux: boolean,
  pause: boolean,
  errors: {
    name: string,
    cityName: string,
    gmtDiff: string
  }
}

class TimeZones extends React.PureComponent<TimeZonesProps, TimeZonesState> {

  private intervalId: NodeJS.Timeout | undefined = undefined;

  constructor(props: TimeZonesProps) {
    super(props);

    this.state = {
      aux: false, pause: false, tableref: React.createRef(),
      errors: {
        name: '',
        cityName: '',
        gmtDiff: ''
      }
    }

    this.ensureDataFetched = this.ensureDataFetched.bind(this);
    this.handleChange = this.handleChange.bind(this);
    this.updateState = this.updateState.bind(this);
    this.activatePause = this.activatePause.bind(this);
    this.validateTimeZoneData = this.validateTimeZoneData.bind(this);
  }

  private readonly columns = [
    { title: 'Key', field: 'key', editable: 'never' as const, hidden: true },
    { title: 'Name', field: 'name', editable: 'always' as const },
    { title: 'City name', field: 'cityName', editable: 'always' as const },
    { title: 'GMT +-', field: 'gmtDiff', editable: 'always' as const },
    { title: 'Time there', field: 'timeThere', editable: 'never' as const },
    { title: 'Local +-', field: 'localDiff', editable: 'never' as const },
    { title: 'Owner username', field: 'ownerUsername', editable: 'never' as const },
  ]

  private GenerateDataRow(timezone: TimeZoneDto): { key: string, name: string; cityName: string; gmtDiff: number; timeThere: string; localDiff: number; ownerUsername: string | undefined; } {
    return {
      key: timezone.key ? timezone.key : '',
      name: timezone.name,
      cityName: timezone.cityName,
      gmtDiff: timezone.gmtDifferenceInHours,
      timeThere: moment.utc().add(timezone.gmtDifferenceInHours, 'hours').format("YYYY-MM-DD, HH:mm:ss"),
      localDiff: 60 * timezone.gmtDifferenceInHours - moment().utcOffset(),
      ownerUsername: timezone.ownerUsername
    };
  }

  // // This method is called when the component is first added to the document
  public componentDidMount() {
    this.ensureDataFetched();
    this.intervalId = setInterval(this.updateState, 1000)
  }

  public componentWillUnmount() {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }

  private updateState() {
    if (!this.state.pause && (!this.state.tableref.current || !this.state.tableref.current.state.lastEditingRow)) {
      this.setState({
        aux: !this.state.aux
      })
    }
  }

  public handleChange(e: any): void {
    this.ensureDataFetched();
  }

  private ensureDataFetched() {
    this.setState({ pause: false });
    this.props.requestTimeZones();
  }

  public render() {
    return (
      <React.Fragment>
        <h1 id="tabelLabel">TimeZones</h1>
        <div onClick={this.handleChange}>
          <Button
            value={'Refresh'}
            type={'submit'} />
        </div>
        {this.renderTimeZonesTable()}
        {this.renderError()}
      </React.Fragment>
    );
  }

  private renderError() {
    return (
      <div>
        <div className={"panelRowError"}>
          <ul className="ul">
            <li>{this.state.errors.name}</li>
            <li>{this.state.errors.cityName}</li>
            <li>{this.state.errors.gmtDiff}</li>
          </ul>
        </div>

        <div className={"panelRowError"}>
          <span>{this.props.errorMessage}</span>
        </div>
      </div>)
  }

  private renderTimeZonesTable() {
    if (this.props.isLoading) {
      return <CircularProgress />;
    }

    return (
      <MaterialTable
        tableRef={this.state.tableref}
        title={'Existing timezones'}
        icons={tableIcons}
        columns={this.columns}
        data={this.props.timezones.map((timezone: TimeZoneDto) => {
          return this.GenerateDataRow(timezone)
        })}
        editable={{
          onRowAdd: newData =>
            this.onRowAdd(newData),
          onRowUpdate: (newData, oldData) =>
            this.onRowUpdate(newData, oldData),
          onRowDelete: oldData =>
            this.onRowDelete(oldData)
        }}
        onRowClick={this.activatePause}
      />);
  }

  private activatePause() {
    // Doesn't fully serve us as workaround
    // this.setState({ pause: !this.state.pause });
  }

  private validateTimeZoneData(data: rowType) {
    let errors = {
      name: '',
      cityName: '',
      gmtDiff: ''
    }

    errors.name = !data.name
      ? 'Please select a name'
      : '';

    errors.cityName = !data.cityName
      ? 'Please select a city'
      : '';

    errors.gmtDiff = !data.gmtDiff || (!Number(data.gmtDiff) && Number(data.gmtDiff) !== 0)
      ? 'Please select a GMT diff (a number is required)'
      : '';

    this.setState({ errors: errors });

    return errors;
  }

  private onRowAdd(newData: rowType): Promise<void> {
    let errors = this.validateTimeZoneData(newData);

    if (errors.name || errors.cityName || errors.gmtDiff) {
      return new Promise(resolve => resolve());
    }

    return new Promise(resolve => {
      setTimeout(() => {
        let timeZone: TimeZoneDto = this.generateTimeZoneDto(newData);
        this.props.createTimeZone(timeZone);
        resolve();
      }, 1000);
    });
  }

  private onRowUpdate(
    newData: { key: string | undefined, name: string; cityName: string; gmtDiff: number; timeThere: string; localDiff: number; ownerUsername: string | undefined; },
    oldData: { key: string | undefined, name: string; cityName: string; gmtDiff: number; timeThere: string; localDiff: number; ownerUsername: string | undefined; } | undefined): Promise<void> {
    let errors = this.validateTimeZoneData(newData);

    if (errors.name || errors.cityName || errors.gmtDiff) {
      return new Promise(resolve => resolve());
    }

    return new Promise(resolve => {
      setTimeout(() => {
        if (oldData) {
          let timeZoneKey: string = this.extractSelectedTimeZoneKey(oldData);
          if (timeZoneKey) {
            let timeZone: TimeZoneDto = this.generateTimeZoneDto(newData);
            timeZone.key = timeZoneKey;
            this.props.updateTimeZone(timeZone);
          }
        }
        resolve();
      }, 1000);
    });
  }

  private onRowDelete(oldData: rowType): Promise<void> {
    return new Promise(resolve => {
      setTimeout(() => {
        let timeZoneKey: string = this.extractSelectedTimeZoneKey(oldData);
        this.props.deleteTimeZone(timeZoneKey);
        resolve();
      }, 1000);
    });
  }

  private extractSelectedTimeZoneKey(oldData: rowType) {
    let timeZoneKey: string = '';
    if (oldData.key) {
      timeZoneKey = oldData.key;
    }
    else {
      let filteredTimeZone = this.props.timezones.filter(tz => tz.name === oldData.name);
      if (filteredTimeZone.length > 0 && filteredTimeZone[0].key) {
        timeZoneKey = filteredTimeZone[0].key;
      }
    }
    return timeZoneKey;
  }

  private generateTimeZoneDto(newData: rowType): TimeZoneDto {
    return {
      name: newData.name,
      cityName: newData.cityName,
      gmtDifferenceInHours: newData.gmtDiff
    };
  }
}

export default connect(
  (state: ApplicationState) => state.timezones, // Selects which state properties are merged into the component's props
  TimeZonesStore.actionCreators // Selects which action creators are merged into the component's props
)(TimeZones as any);
