import { Injectable } from '@angular/core';
import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { PowerUpType } from '../clients/web-api-clients';
import { IPowerUpGroup } from '../models/power-up-group';
import { PowerUpGroupType, POWER_UP_GROUP_TYPES, POWER_UP_TYPES } from '../enums/power-up-group-type';
import { IGlobalPowerUpGroup } from '../models/global-power-up-group';
import { PowerUpDataMap } from '../constants/power-up-data-map';
import { PowerUpData } from '../models/power-up-data';

@Injectable({
  providedIn: 'root'
})
export class PowerUpsService {

  constructor() { }


  powerUpDatas(): PowerUpData[] {
    return Object.values(PowerUpDataMap)
  }

  powerUpTypes(): PowerUpType[] {
    return Object.keys(PowerUpDataMap).map(k => +k) as PowerUpType[];
  }

  async loadImages() {
    const images = this.powerUpDatas().map(data => this.loadImage(data))
    await Promise.all(images);
  }

  private loadImage(data: PowerUpData): Promise<void> {
    return new Promise((resolve, reject) => {
      const image = new Image();
      image.onload = () => {
        data.icon = image;
        resolve()
      };
      image.onerror = (error) => reject(error);
      image.src = data.iconPath;
    });
  }

  getPowerUpsAsFormGroup() {
    const obj = Object.fromEntries(POWER_UP_TYPES.map(type => ([type, new FormControl(false)])))
    return new FormGroup(obj);
  }

  getData(type: PowerUpType): PowerUpData | undefined { return PowerUpDataMap[type]; }
}
