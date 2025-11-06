import { Self } from "@angular/core";
import { ControlValueAccessor, FormControl, NgControl } from "@angular/forms";

export abstract class BaseFormFieldValueAccessor implements ControlValueAccessor {
  private _value: any = '';
  private disabled = false;

  constructor(@Self() public controlDir: NgControl) {
    this.controlDir.valueAccessor = this;
  }

  get control(): FormControl {
    return this.controlDir.control as FormControl;
  }

  private onChange: (value: any) => void = () => {};
  private onTouched: () => void = () => {};

  writeValue(obj: any): void {
    if (obj !== undefined && obj !== null) {
      this._value = obj;
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  handleChange(value: any): void {
    this._value = value;
    this.onChange(value);
  }

  handleBlur(): void {
    this.onTouched();
  }
}
