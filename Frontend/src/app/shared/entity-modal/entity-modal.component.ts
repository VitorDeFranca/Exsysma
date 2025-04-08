import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

export interface FieldConfig {
    key: string;
    label: string;
    type: 'text' | 'number' | 'email' | 'date' | 'select' | 'textarea';
    required: boolean;
    options?: { value: any; label: string }[]; // For select inputs
    validators?: any[];
    defaultValue?: any;
    hint?: string;
}

@Component({
  selector: 'app-entity-modal',
  templateUrl: './entity-modal.component.html',
  styleUrls: ['./entity-modal.component.css']
})
export class EntityModalComponent implements OnInit {
  @Input() title: string = 'Entity';
  @Input() icon: string = 'edit'; // Material icon name
  @Input() fields: FieldConfig[] = [];
  @Input() submitButtonText: string = 'Save';

  entityForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<EntityModalComponent>
  ) {
    this.entityForm = this.formBuilder.group({});
  }

  ngOnInit(): void {
    this.createForm();
  }

  createForm(): void {
    const formGroupConfig: { [key: string]: any } = {};

    console.log(this.fields)

    this.fields.forEach(field => {
      const validators = field.required ? [Validators.required] : [];

      if (field.validators) {
        validators.push(...field.validators);
      }

      formGroupConfig[field.key] = [field.defaultValue || '', validators];
    });

    this.entityForm = this.formBuilder.group(formGroupConfig);
  }

  getErrorMessage(fieldName: string): string {
    const field = this.entityForm.get(fieldName);

    if (!field) return '';

    if (field.hasError('required')) {
      return 'This field is required';
    }

    if (field.hasError('email')) {
      return 'Please enter a valid email address';
    }

    if (field.hasError('minlength')) {
      const minLength = field.errors?.['minlength'].requiredLength;
      return `Minimum length is ${minLength} characters`;
    }

    return 'Invalid input';
  }

  onSubmit(): void {
    if (this.entityForm.valid) {
      this.dialogRef.close(this.entityForm.value);
    } else {
      this.markFormGroupTouched(this.entityForm);
    }
  }

  cancel(): void {
    this.dialogRef.close();
  }

  // Helper to mark all controls as touched to trigger validation
  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }}
