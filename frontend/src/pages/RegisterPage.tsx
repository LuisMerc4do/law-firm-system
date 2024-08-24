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
      message: "Please enter a valid email address.",
    }),
    firstName: z.string().min(2, {
      message: "Please enter your first name.",
    }),
    lastName: z.string().min(2, {
      message: "Please enter your last name.",
    }),
    phoneNumber: z.string().min(8, {
      message: "Please enter your phone number.",
    }),
    password: z.string().min(8, {
      message: "Password must be at least 8 characters long.",
    }),
    confirmPassword: z.string(),
    terms: z.boolean().refine((val) => val === true, {
      message: "You must accept the terms and conditions.",
    }),
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: "Passwords do not match.",
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
      toast.error("Error during registration.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-100">
      <Card className="w-full max-w-md">
        <CardHeader>
          <CardTitle>Register</CardTitle>
          <CardDescription>
            Create an account to manage your legal cases.
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
                    <FormLabel>Email address</FormLabel>
                    <FormControl>
                      <Input placeholder="name@example.com" {...field} />
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
                    <FormLabel>Your first name</FormLabel>
                    <FormControl>
                      <Input placeholder="Your first name" {...field} />
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
                    <FormLabel>Your last name</FormLabel>
                    <FormControl>
                      <Input placeholder="Your last name" {...field} />
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
                    <FormLabel>Your phone number</FormLabel>
                    <FormControl>
                      <Input placeholder="Your phone number" {...field} />
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
                    <FormLabel>Password</FormLabel>
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
                    <FormLabel>Confirm password</FormLabel>
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
                      <FormLabel>I accept the terms and conditions</FormLabel>
                      <FormDescription>
                        By creating an account, you agree to our{" "}
                        <a href="#" className="text-primary hover:underline">
                          Terms of Service
                        </a>{" "}
                        and{" "}
                        <a href="#" className="text-primary hover:underline">
                          Privacy Policy
                        </a>
                        .
                      </FormDescription>
                    </div>
                  </FormItem>
                )}
              />
              <Button type="submit" className="w-full" disabled={isLoading}>
                {isLoading ? "Registering..." : "Register"}
              </Button>
            </form>
          </Form>
        </CardContent>
        <CardFooter className="flex justify-center">
          <p className="text-sm text-muted-foreground">
            Already have an account?{" "}
            <a href="#" className="text-primary hover:underline">
              Log in
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
  if (score === 0) strength = "Very weak";
  else if (score === 1) strength = "Weak";
  else if (score === 2) strength = "Acceptable";
  else if (score === 3) strength = "Strong";
  else if (score >= 4) strength = "Very strong";

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

interface PasswordStrengthIndicatorProps {
  password: string;
}

function PasswordStrengthIndicator({
  password,
}: PasswordStrengthIndicatorProps) {
  const strength = passwordStrength(password);

  return (
    <div className="mt-2">
      <p
        className={cn("text-sm font-medium", {
          "text-red-600": strength.score <= 1,
          "text-yellow-600": strength.score === 2,
          "text-green-600": strength.score >= 3,
        })}
      >
        {strength.strength}
      </p>
      <ul className="mt-1 space-y-1 text-sm text-muted-foreground">
        <li
          className={cn({
            "text-green-600": strength.checks.length,
            "line-through text-muted-foreground": !strength.checks.length,
          })}
        >
          • At least 8 characters
        </li>
        <li
          className={cn({
            "text-green-600": strength.checks.hasUpperCase,
            "line-through text-muted-foreground": !strength.checks.hasUpperCase,
          })}
        >
          • At least 1 uppercase letter
        </li>
        <li
          className={cn({
            "text-green-600": strength.checks.hasLowerCase,
            "line-through text-muted-foreground": !strength.checks.hasLowerCase,
          })}
        >
          • At least 1 lowercase letter
        </li>
        <li
          className={cn({
            "text-green-600": strength.checks.hasNumber,
            "line-through text-muted-foreground": !strength.checks.hasNumber,
          })}
        >
          • At least 1 number
        </li>
        <li
          className={cn({
            "text-green-600": strength.checks.hasSpecialChar,
            "line-through text-muted-foreground":
              !strength.checks.hasSpecialChar,
          })}
        >
          • At least 1 special character
        </li>
      </ul>
    </div>
  );
}
