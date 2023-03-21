import { BorderService } from "../elements/border.service";
import { EngineService } from "../engine.service";
import { CoodrinatesService } from "../helpers/coodrinates.service";
import { KeysListenerService } from "../helpers/keys-listener.service";
import { DrawerService } from "../helpers/drawer.service";
import { PowerUpsService } from "../elements/power-ups.service";
import { PlayersService } from "../elements/players.service";

export const HELPER_SERVICES = [
    EngineService,
    BorderService,
    CoodrinatesService,
    KeysListenerService,
    DrawerService,
    PowerUpsService,
    PlayersService
]