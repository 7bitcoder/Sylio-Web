import { Observable, ObservableInput, ObservedValueOf, OperatorFunction, catchError, from, map, of, startWith, switchMap } from "rxjs";
import { Result } from "../models/result";

export function switchMapWithInput<T, O extends ObservableInput<any>>(
    project: (value: T, index: number) => O): OperatorFunction<T, readonly [T, ObservedValueOf<O>]> {
    return (source: Observable<T>) => source.pipe(
        switchMap((a, i) => from(project(a, i)).pipe(
            map(b => [a, b] as const)
        ))
    );
}

export function switchMapWithResult<T, O>(
    project: (value: T, index: number) => Observable<O>) {
    return (source: Observable<T>) => source.pipe(
        switchMap((a, i) => project(a, i).pipe(
            map(value => <Result<O>>({ loading: false, value })),
            catchError(error => of(<Result<O>>({ loading: false, error }))),
            startWith(<Result<O>>({ loading: true }))
        ))
    );
}