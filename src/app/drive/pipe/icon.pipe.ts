import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'icon'
})
export class IconPipe implements PipeTransform {

  transform(extension?: string | null) {
    let res: string = '';
    switch (extension) {
      case null:
        return "fa-solid fa-folder";
      default:
        return "fa-solid fa-file";
    }

  }

}
