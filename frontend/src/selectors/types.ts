import { OutputSelector, OutputParametricSelector } from 'reselect';
import { ApplicationState } from '../store';

export type Selector<TInput, TOutput> = OutputSelector<ApplicationState, TOutput, (res: TInput) => TOutput>;
export type Selector2<TParam, TInput1, TInput2, TOutput> = OutputParametricSelector<ApplicationState, TParam, TOutput, (res1: TInput1, res2: TInput2) => TOutput>;
