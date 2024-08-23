import { useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { Eye, EyeOff, Check, X } from "lucide-react";

import { cn } from "../lib/utils";
import { Button } from "../components/ui/button";
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "../components/ui/form";
import { Input } from "../components/ui/input";
import { Checkbox } from "../components/ui/checkbox";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "../components/ui/card";
import toast from "react-hot-toast";
import { useAuth } from "../context/useAuth";

// Schema for form validation
const formSchema = z
  .object({
    email: z.string().email({
      message: "Por favor ingrese un correo electrónico válido.",
    }),
    firstName: z.string().min(2, {
      message: "Por favor ingrese su Nombre.",
    }),
    lastName: z.string().min(2, {
      message: "Por favor ingrese su Apellido.",
    }),
    phoneNumber: z.string().min(8, {
      message: "Por favor ingrese su Numero de telefono.",
    }),
    password: z.string().min(8, {
      message: "La contraseña debe tener al menos 8 caracteres.",
    }),
    confirmPassword: z.string(),
    terms: z.boolean().refine((val) => val === true, {
      message: "Debe aceptar los términos y condiciones.",
    }),
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: "Las contraseñas no coinciden.",
    path: ["confirmPassword"],
  });

export default function RegisterPage() {
  const [showPassword, setShowPassword] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const { registerUser } = useAuth();

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      email: "",
      firstName: "",
      lastName: "",
      phoneNumber: "",
      password: "",
      confirmPassword: "",
      terms: false,
    },
  });

  const onSubmit = async (values: z.infer<typeof formSchema>) => {
    setIsLoading(true);
    try {
      await registerUser(
        values.email,
        values.email,
        values.password,
        values.firstName,
        values.lastName,
        values.phoneNumber
      );
    } catch (err) {
      toast.error("Error al Iniciar Sesion");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-100">
      <Card className="w-full max-w-md">
        <CardHeader>
          <CardTitle>Registro</CardTitle>
          <CardDescription>
            Cree una cuenta para gestionar sus casos legales.
          </CardDescription>
        </CardHeader>
        <CardContent>
          <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
              <FormField
                control={form.control}
                name="email"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Correo electrónico</FormLabel>
                    <FormControl>
                      <Input placeholder="nombre@ejemplo.com" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="firstName"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Tu nombre</FormLabel>
                    <FormControl>
                      <Input placeholder="tu nombre" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="lastName"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Tus Apellidos</FormLabel>
                    <FormControl>
                      <Input placeholder="tu nombre" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="phoneNumber"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Tu numero de Telefono</FormLabel>
                    <FormControl>
                      <Input placeholder="tu nombre" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="password"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Contraseña</FormLabel>
                    <FormControl>
                      <div className="relative">
                        <Input
                          type={showPassword ? "text" : "password"}
                          {...field}
                        />
                        <Button
                          type="button"
                          variant="ghost"
                          size="sm"
                          className="absolute right-0 top-0 h-full px-3 py-2 hover:bg-transparent"
                          onClick={() => setShowPassword(!showPassword)}
                        >
                          {showPassword ? (
                            <EyeOff className="h-4 w-4" />
                          ) : (
                            <Eye className="h-4 w-4" />
                          )}
                        </Button>
                      </div>
                    </FormControl>
                    <PasswordStrengthIndicator password={field.value} />
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="confirmPassword"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Confirmar contraseña</FormLabel>
                    <FormControl>
                      <Input type="password" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="terms"
                render={({ field }) => (
                  <FormItem className="flex flex-row items-start space-x-3 space-y-0">
                    <FormControl>
                      <Checkbox
                        checked={field.value}
                        onCheckedChange={field.onChange}
                      />
                    </FormControl>
                    <div className="space-y-1 leading-none">
                      <FormLabel>Acepto los términos y condiciones</FormLabel>
                      <FormDescription>
                        Al crear una cuenta, usted acepta nuestros{" "}
                        <a href="#" className="text-primary hover:underline">
                          Términos de servicio
                        </a>{" "}
                        y{" "}
                        <a href="#" className="text-primary hover:underline">
                          Política de privacidad
                        </a>
                        .
                      </FormDescription>
                    </div>
                  </FormItem>
                )}
              />
              <Button type="submit" className="w-full" disabled={isLoading}>
                {isLoading ? "Registrando..." : "Registrarse"}
              </Button>
            </form>
          </Form>
        </CardContent>
        <CardFooter className="flex justify-center">
          <p className="text-sm text-muted-foreground">
            ¿Ya tiene una cuenta?{" "}
            <a href="#" className="text-primary hover:underline">
              Iniciar sesión
            </a>
          </p>
        </CardFooter>
      </Card>
    </div>
  );
}
function passwordStrength(password: string) {
  // Initialize score and checks object
  let score = 0;
  const checks = {
    length: false,
    hasUpperCase: false,
    hasLowerCase: false,
    hasNumber: false,
    hasSpecialChar: false,
  };

  // Check length
  if (password.length >= 8) {
    score++;
    checks.length = true;
  }

  // Check for uppercase letters
  if (/[A-Z]/.test(password)) {
    score++;
    checks.hasUpperCase = true;
  }

  // Check for lowercase letters
  if (/[a-z]/.test(password)) {
    score++;
    checks.hasLowerCase = true;
  }

  // Check for numbers
  if (/\d/.test(password)) {
    score++;
    checks.hasNumber = true;
  }

  // Check for special characters
  if (/[!@#$%^&*(),.?":{}|<>]/.test(password)) {
    score++;
    checks.hasSpecialChar = true;
  }

  // Determine strength label
  let strength = "";
  if (score === 0) strength = "Muy débil";
  else if (score === 1) strength = "Débil";
  else if (score === 2) strength = "Aceptable";
  else if (score === 3) strength = "Fuerte";
  else if (score >= 4) strength = "Muy fuerte";

  // Return the result
  return {
    score,
    checks,
    strength,
  };
}

// Test the function with different passwords
const testPasswords = [
  "",
  "password",
  "Password1",
  "P@ssw0rd",
  "Str0ng!P@ssw0rd",
];

testPasswords.forEach((password) => {
  const result = passwordStrength(password);
  console.log(`Password: ${password}`);
  console.log(`Strength: ${result.strength} (Score: ${result.score})`);
  console.log("Checks:", result.checks);
  console.log("---");
});

function PasswordStrengthIndicator({ password }: { password: string }) {
  const strength = passwordStrength(password);
  return (
    <div className="mt-2">
      <div className="flex justify-between mb-1">
        <span className="text-sm font-medium text-muted-foreground">
          Fortaleza de la contraseña
        </span>
        <span className="text-sm font-medium text-muted-foreground">
          {strength.score === 0
            ? "Muy débil"
            : strength.score === 1
            ? "Débil"
            : strength.score === 2
            ? "Aceptable"
            : strength.score === 3
            ? "Fuerte"
            : "Muy fuerte"}
        </span>
      </div>
      <div className="w-full bg-gray-200 rounded-full h-2.5">
        <div
          className={cn(
            "h-2.5 rounded-full",
            strength.score === 0 && "w-0",
            strength.score === 1 && "w-1/4 bg-red-500",
            strength.score === 2 && "w-2/4 bg-yellow-500",
            strength.score === 3 && "w-3/4 bg-blue-500",
            strength.score >= 4 && "w-full bg-green-500"
          )}
        ></div>
      </div>
      <ul className="mt-2 grid grid-cols-2 gap-2 text-sm">
        <li
          className={cn(
            "flex items-center",
            strength.checks.length ? "text-green-500" : "text-muted-foreground"
          )}
        >
          <Check
            className={cn(
              "mr-1 h-4 w-4",
              !strength.checks.length && "text-muted-foreground"
            )}
          />{" "}
          8+ caracteres
        </li>
        <li
          className={cn(
            "flex items-center",
            strength.checks.hasUpperCase
              ? "text-green-500"
              : "text-muted-foreground"
          )}
        >
          <Check
            className={cn(
              "mr-1 h-4 w-4",
              !strength.checks.hasUpperCase && "text-muted-foreground"
            )}
          />{" "}
          Mayúscula
        </li>
        <li
          className={cn(
            "flex items-center",
            strength.checks.hasLowerCase
              ? "text-green-500"
              : "text-muted-foreground"
          )}
        >
          <Check
            className={cn(
              "mr-1 h-4 w-4",
              !strength.checks.hasLowerCase && "text-muted-foreground"
            )}
          />{" "}
          Minúscula
        </li>
        <li
          className={cn(
            "flex items-center",
            strength.checks.hasNumber
              ? "text-green-500"
              : "text-muted-foreground"
          )}
        >
          <Check
            className={cn(
              "mr-1 h-4 w-4",
              !strength.checks.hasNumber && "text-muted-foreground"
            )}
          />{" "}
          Número
        </li>
        <li
          className={cn(
            "flex items-center",
            strength.checks.hasSpecialChar
              ? "text-green-500"
              : "text-muted-foreground"
          )}
        >
          <Check
            className={cn(
              "mr-1 h-4 w-4",
              !strength.checks.hasSpecialChar && "text-muted-foreground"
            )}
          />{" "}
          Carácter especial
        </li>
      </ul>
    </div>
  );
}
