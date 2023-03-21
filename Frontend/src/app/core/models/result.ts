export interface Result<T> {
    value?: T,
    loading: boolean,
    error?: any
}