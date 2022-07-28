using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AwIngenieriaSoftware.Models
{
    public class Identificacion : ValidationAttribute
    {
        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //{
        //    int isNumeric;
        //    var total = 0;
        //    const int tamanoLongitudCedula = 10;
        //    int[] coeficientes = { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
        //    const int numeroProvincias = 24;
        //    const int tercerDigito = 6;
        //    if (value != null)
        //    {
        //        String cedula = value.ToString();
        //        if (Regex.IsMatch(cedula, @"[0-9]"))
        //        {
        //            if (cedula.Length == tamanoLongitudCedula)
        //            {
        //                var provincia = Convert.ToInt32(string.Concat(cedula[0], cedula[1], String.Empty));
        //                var digitoTres = Convert.ToInt32(cedula[2].ToString());
        //                if ((provincia > 0 && provincia <= numeroProvincias) && digitoTres < tercerDigito)
        //                {
        //                    var digitoVerificadorRecibo = Convert.ToInt32(cedula[9].ToString());
        //                    for (int k = 0; k < coeficientes.Length; k++)
        //                    {
        //                        var valor = Convert.ToInt32(coeficientes[k].ToString()) * Convert.ToInt32(cedula[k].ToString());
        //                        total = valor >= 10 ? total + (valor - 9) : total + valor;
        //                    }
        //                    var digitoVerificadorObtenido = total >= 10 ? (total % 10) != 0 ? 10 - (total % 10) : (total % 10) : total;
        //                    if (digitoVerificadorObtenido == digitoVerificadorRecibo)
        //                    {
        //                        return ValidationResult.Success;
        //                    }
        //                    else
        //                    {
        //                        return new ValidationResult("Número de identificación incorrecto");
        //                    }
        //                }
        //                else
        //                {
        //                    return new ValidationResult("Número de identificación incorrecto");
        //                }
        //            }
        //            else
        //            {
        //                return new ValidationResult("Longitud de cédula incorrecta");
        //            }
        //        }
        //        else
        //        {
        //            return ValidationResult.Success;
        //        }
        //    }
        //    else
        //    {
        //        return new ValidationResult("El campo " + validationContext.DisplayName + " es obligatorio");
        //    }
        //}

        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool estado = false;
            char[] valced = new char[13];
            int provincia;
            String identificacion = (value != null) ? value.ToString() : "";
            if (identificacion.Length >= 10)
            {
                if (Regex.IsMatch(identificacion, @"[0-9]"))
                {
                    valced = identificacion.Trim().ToCharArray();
                    provincia = int.Parse((valced[0].ToString() + valced[1].ToString()));
                    if (provincia > 0 && provincia < 25)
                    {
                        if (int.Parse(valced[2].ToString()) < 6)
                        {
                            estado = VerificaCedula(valced);
                        }
                        else if (int.Parse(valced[2].ToString()) == 6)
                        {
                            estado = VerificaSectorPublico(valced);
                        }
                        else if (int.Parse(valced[2].ToString()) == 9)
                        {

                            estado = VerificaPersonaJuridica(valced);
                        }
                        if (estado)
                        {
                            return ValidationResult.Success;
                        }
                        else
                        {
                            return new ValidationResult("Número de identificación incorrecto");
                        }
                    }
                    else
                    {
                        return new ValidationResult("Número de identificación incorrecto");
                    }
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return new ValidationResult("El campo " + validationContext.DisplayName + " es obligatorio");
            }
        }

        public static bool VerificaCedula(char[] validarCedula)
        {
            int aux = 0, par = 0, impar = 0, verifi;
            for (int i = 0; i < 9; i += 2)
            {
                aux = 2 * int.Parse(validarCedula[i].ToString());
                if (aux > 9)
                    aux -= 9;
                par += aux;
            }
            for (int i = 1; i < 9; i += 2)
            {
                impar += int.Parse(validarCedula[i].ToString());
            }

            aux = par + impar;
            if (aux % 10 != 0)
            {
                verifi = 10 - (aux % 10);
            }
            else
                verifi = 0;
            if (verifi == int.Parse(validarCedula[9].ToString()))
                return true;
            else
                return false;
        }
        public static bool VerificaPersonaJuridica(char[] validarCedula)
        {
            int aux = 0, prod, veri;
            veri = int.Parse(validarCedula[10].ToString()) + int.Parse(validarCedula[11].ToString()) + int.Parse(validarCedula[12].ToString());
            if (veri > 0)
            {
                int[] coeficiente = new int[9] { 4, 3, 2, 7, 6, 5, 4, 3, 2 };
                for (int i = 0; i < 9; i++)
                {
                    prod = int.Parse(validarCedula[i].ToString()) * coeficiente[i];
                    aux += prod;
                }
                if (aux % 11 == 0)
                {
                    veri = 0;
                }
                else if (aux % 11 == 1)
                {
                    return false;
                }
                else
                {
                    aux = aux % 11;
                    veri = 11 - aux;
                }

                if (veri == int.Parse(validarCedula[9].ToString()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public static bool VerificaSectorPublico(char[] validarCedula)
        {
            int aux = 0, prod, veri;
            veri = int.Parse(validarCedula[9].ToString()) + int.Parse(validarCedula[10].ToString()) + int.Parse(validarCedula[11].ToString()) + int.Parse(validarCedula[12].ToString());
            if (veri > 0)
            {
                int[] coeficiente = new int[8] { 3, 2, 7, 6, 5, 4, 3, 2 };

                for (int i = 0; i < 8; i++)
                {
                    prod = int.Parse(validarCedula[i].ToString()) * coeficiente[i];
                    aux += prod;
                }

                if (aux % 11 == 0)
                {
                    veri = 0;
                }
                else if (aux % 11 == 1)
                {
                    return false;
                }
                else
                {
                    aux = aux % 11;
                    veri = 11 - aux;
                }

                if (veri == int.Parse(validarCedula[8].ToString()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}