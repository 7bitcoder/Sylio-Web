import { Pipe, PipeTransform } from '@angular/core';
import { PowerUpType } from 'src/app/core/clients/web-api-clients';
import { PowerUpGroupType } from 'src/app/core/enums/power-up-group-type';
import { PowerUpsService } from 'src/app/core/services/power-ups.service copy';

@Pipe({
  name: 'powerUpIcon'
})
export class PowerUpIconPipe implements PipeTransform {

  constructor(private powerUpService: PowerUpsService) { }

  transform(value: PowerUpType): string {
    return this.powerUpService.getData(value)?.iconPath ?? '';
  }
}
