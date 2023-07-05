import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[appFileManager]'
})
export class FileManagerDirective {

  constructor(public viewContainerRef: ViewContainerRef) { }

}
