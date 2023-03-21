import { IDrawable } from "./drawable";
import { IUpdatable } from "./updatable";

export interface IElement extends IUpdatable, IDrawable {
}