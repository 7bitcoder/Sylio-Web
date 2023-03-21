import { NotificationType } from "./notification-type";

export interface GameNotification {
    type: NotificationType,
    payload: any
};