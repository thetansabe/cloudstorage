export interface DisplayNotification {
    title: string;
    message: string;
    type: NotificationType;
    id: number;
}

export enum NotificationType {
    INFO = 0,
    SUCCESS = 1,
    FAILED = 2,
    WARNING = 3
}