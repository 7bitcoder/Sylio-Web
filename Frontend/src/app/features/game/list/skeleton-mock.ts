import { GameDto } from "src/app/core/clients/web-api-clients";

export const SKELETON_MOCK = Array.from({ length: 4 }, () => new GameDto({
    created: new Date(),
    title: 'mock',
    isPublic: true,
    maxPlayers: 8,
    players: 4,
    roundsNumber: 20,
    state: 0
}));