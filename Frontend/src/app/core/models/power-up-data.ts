import { PowerUpType } from "../clients/web-api-clients";

export interface PowerUpData {
    type: PowerUpType,
    iconPath: string,
    icon?: HTMLImageElement,
    description: string,
}