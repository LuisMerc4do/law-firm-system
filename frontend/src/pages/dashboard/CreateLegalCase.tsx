import React, { useState } from "react";
import { useForm, SubmitHandler, ControllerRenderProps } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { format } from "date-fns";
import axios, { AxiosError } from "axios";
import { useAuth } from "../../context/useAuth";
import { toast } from "react-hot-toast";

import { cn } from "../../lib/utils";
import { Button } from "../../components/ui/button";
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "../../components/ui/form";
import { Input } from "../../components/ui/input";
import { Textarea } from "../../components/ui/textarea";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "../../components/ui/select";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "../../components/ui/popover";
import { Calendar } from "../../components/ui/calendar";
import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "../../components/ui/card";
import { Slider } from "../../components/ui/slider";
import { Switch } from "../../components/ui/switch";
import { Badge } from "../../components/ui/badge";
import { CalendarIcon, DollarSign, Clock, Scale } from "lucide-react";

const API_URL = "http://localhost:5224/api/v1";

const formSchema = z.object({
  title: z.string().min(5, {
    message: "Case title must be at least 5 characters.",
  }),
  clientName: z.string().min(2, {
    message: "Client name must be at least 2 characters.",
  }),
  caseType: z.string({
    required_error: "Please select a case type.",
  }),
  description: z.string().min(20, {
    message: "Case description must be at least 20 characters.",
  }),
  incidentDate: z.date({
    required_error: "Incident date is required.",
  }),
  filingDate: z.date({
    required_error: "Filing date is required.",
  }),
  courtName: z.string().min(2, {
    message: "Court name must be at least 2 characters.",
  }),
  opposingParty: z.string().min(2, {
    message: "Opposing party must be at least 2 characters.",
  }),
  estimatedDuration: z.number().min(1).max(60),
  caseValue: z.number().min(1000),
  priority: z.number().min(1).max(5),
  isConfidential: z.boolean(),
});

type FormValues = z.infer<typeof formSchema>;

interface CaseType {
  value: string;
  label: string;
}

const caseTypes: CaseType[] = [
  { value: "civil", label: "Civil Litigation" },
  { value: "criminal", label: "Criminal Defense" },
  { value: "family", label: "Family Law" },
  { value: "corporate", label: "Corporate Law" },
  { value: "intellectual_property", label: "Intellectual Property" },
  { value: "real_estate", label: "Real Estate Law" },
];

const CreateLegalCase: React.FC = () => {
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const { user, token } = useAuth();

  const form = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      title: "",
      clientName: "",
      description: "",
      courtName: "",
      opposingParty: "",
      estimatedDuration: 1,
      caseValue: 1000,
      priority: 3,
      isConfidential: false,
    },
  });

  const onSubmit: SubmitHandler<FormValues> = async (values) => {
    setIsLoading(true);
    try {
      const response = await axios.post(
        `${API_URL}/cases`,
        {
          ...values,
          createdById: user?.userName,
          assignedToId: user?.userName,
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      toast.success("Case created successfully!");
      form.reset();
    } catch (error) {
      if (axios.isAxiosError(error)) {
        toast.error(
          error.response?.data || "An error occurred while creating the case."
        );
      } else {
        toast.error("An unexpected error occurred.");
      }
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <>
      <Card className="w-full max-w-7xl mx-auto my-8">
        <CardHeader>
          <CardTitle className="text-3xl font-bold text-center">
            Create New Legal Case
          </CardTitle>
        </CardHeader>
        <CardContent>
          <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-8">
              <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                {/* Left Column */}
                <div className="space-y-6">
                  <FormField
                    control={form.control}
                    name="title"
                    render={({
                      field,
                    }: {
                      field: ControllerRenderProps<FormValues, "title">;
                    }) => (
                      <FormItem>
                        <FormLabel>Case Title</FormLabel>
                        <FormControl>
                          <Input
                            placeholder="Enter case title"
                            {...field}
                            className="bg-secondary"
                          />
                        </FormControl>
                        <FormDescription>
                          Provide a concise title for the case.
                        </FormDescription>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="clientName"
                    render={({
                      field,
                    }: {
                      field: ControllerRenderProps<FormValues, "clientName">;
                    }) => (
                      <FormItem>
                        <FormLabel>Client Name</FormLabel>
                        <FormControl>
                          <Input
                            placeholder="Enter client name"
                            {...field}
                            className="bg-secondary"
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="caseType"
                    render={({
                      field,
                    }: {
                      field: ControllerRenderProps<FormValues, "caseType">;
                    }) => (
                      <FormItem>
                        <FormLabel>Case Type</FormLabel>
                        <Select
                          onValueChange={field.onChange}
                          defaultValue={field.value}
                        >
                          <FormControl>
                            <SelectTrigger>
                              <SelectValue placeholder="Select a case type" />
                            </SelectTrigger>
                          </FormControl>
                          <SelectContent>
                            {caseTypes.map((type) => (
                              <SelectItem key={type.value} value={type.value}>
                                {type.label}
                              </SelectItem>
                            ))}
                          </SelectContent>
                        </Select>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="description"
                    render={({
                      field,
                    }: {
                      field: ControllerRenderProps<FormValues, "description">;
                    }) => (
                      <FormItem>
                        <FormLabel>Case Description</FormLabel>
                        <FormControl>
                          <Textarea
                            placeholder="Provide a detailed description of the case"
                            className="resize-none bg-secondary"
                            {...field}
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="incidentDate"
                    render={({
                      field,
                    }: {
                      field: ControllerRenderProps<FormValues, "incidentDate">;
                    }) => (
                      <FormItem className="flex flex-col">
                        <FormLabel>Incident Date</FormLabel>
                        <Popover>
                          <PopoverTrigger asChild>
                            <FormControl>
                              <Button
                                variant={"outline"}
                                className={cn(
                                  "w-full pl-3 text-left font-normal",
                                  !field.value && "text-muted-foreground"
                                )}
                              >
                                {field.value ? (
                                  format(field.value, "PPP")
                                ) : (
                                  <span>Pick a date</span>
                                )}
                                <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
                              </Button>
                            </FormControl>
                          </PopoverTrigger>
                          <PopoverContent className="w-auto p-0" align="start">
                            <Calendar
                              mode="single"
                              selected={field.value}
                              onSelect={field.onChange}
                              disabled={(date) =>
                                date > new Date() ||
                                date < new Date("1900-01-01")
                              }
                              initialFocus
                            />
                          </PopoverContent>
                        </Popover>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                </div>

                {/* Right Column */}
                <div className="space-y-6">
                  <FormField
                    control={form.control}
                    name="filingDate"
                    render={({
                      field,
                    }: {
                      field: ControllerRenderProps<FormValues, "filingDate">;
                    }) => (
                      <FormItem className="flex flex-col">
                        <FormLabel>Filing Date</FormLabel>
                        <Popover>
                          <PopoverTrigger asChild>
                            <FormControl>
                              <Button
                                variant={"outline"}
                                className={cn(
                                  "w-full pl-3 text-left font-normal",
                                  !field.value && "text-muted-foreground"
                                )}
                              >
                                {field.value ? (
                                  format(field.value, "PPP")
                                ) : (
                                  <span>Pick a date</span>
                                )}
                                <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
                              </Button>
                            </FormControl>
                          </PopoverTrigger>
                          <PopoverContent className="w-auto p-0" align="start">
                            <Calendar
                              mode="single"
                              selected={field.value}
                              onSelect={field.onChange}
                              disabled={(date) =>
                                date > new Date() ||
                                date < new Date("1900-01-01")
                              }
                              initialFocus
                            />
                          </PopoverContent>
                        </Popover>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="courtName"
                    render={({
                      field,
                    }: {
                      field: ControllerRenderProps<FormValues, "courtName">;
                    }) => (
                      <FormItem>
                        <FormLabel>Court Name</FormLabel>
                        <FormControl>
                          <Input
                            placeholder="Enter court name"
                            {...field}
                            className="bg-secondary"
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="opposingParty"
                    render={({
                      field,
                    }: {
                      field: ControllerRenderProps<FormValues, "opposingParty">;
                    }) => (
                      <FormItem>
                        <FormLabel>Opposing Party</FormLabel>
                        <FormControl>
                          <Input
                            placeholder="Enter opposing party name"
                            {...field}
                            className="bg-secondary"
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="estimatedDuration"
                    render={({
                      field,
                    }: {
                      field: ControllerRenderProps<
                        FormValues,
                        "estimatedDuration"
                      >;
                    }) => (
                      <FormItem>
                        <FormLabel>Estimated Duration (in months)</FormLabel>
                        <FormControl>
                          <div className="flex items-center space-x-4">
                            <Slider
                              min={1}
                              max={60}
                              step={1}
                              value={[field.value]}
                              onValueChange={(value: number[]) =>
                                field.onChange(value[0])
                              }
                              className="flex-grow"
                            />
                            <span className="font-bold">
                              {field.value} months
                            </span>
                          </div>
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="caseValue"
                    render={({
                      field,
                    }: {
                      field: ControllerRenderProps<FormValues, "caseValue">;
                    }) => (
                      <FormItem>
                        <FormLabel>Estimated Case Value</FormLabel>
                        <FormControl>
                          <div className="relative">
                            <DollarSign className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" />
                            <Input
                              type="number"
                              placeholder="Enter estimated value"
                              {...field}
                              onChange={(e) =>
                                field.onChange(Number(e.target.value))
                              }
                              className="pl-10 bg-secondary"
                            />
                          </div>
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="priority"
                    render={({
                      field,
                    }: {
                      field: ControllerRenderProps<FormValues, "priority">;
                    }) => (
                      <FormItem>
                        <FormLabel>Priority Level</FormLabel>
                        <FormControl>
                          <div className="flex items-center space-x-4">
                            <Slider
                              min={1}
                              max={5}
                              step={1}
                              value={[field.value]}
                              onValueChange={(value: number[]) =>
                                field.onChange(value[0])
                              }
                              className="flex-grow"
                            />
                            <Badge
                              variant={
                                field.value > 3 ? "destructive" : "secondary"
                              }
                            >
                              {field.value}
                            </Badge>
                          </div>
                        </FormControl>
                        <FormDescription>
                          Set the priority level from 1 (lowest) to 5 (highest)
                        </FormDescription>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="isConfidential"
                    render={({
                      field,
                    }: {
                      field: ControllerRenderProps<
                        FormValues,
                        "isConfidential"
                      >;
                    }) => (
                      <FormItem className="flex flex-row items-center justify-between rounded-lg border p-4">
                        <div className="space-y-0.5">
                          <FormLabel className="text-base">
                            Confidential Case
                          </FormLabel>
                          <FormDescription>
                            Mark this case as confidential if it contains
                            sensitive information.
                          </FormDescription>
                        </div>
                        <FormControl>
                          <Switch
                            checked={field.value}
                            onCheckedChange={field.onChange}
                          />
                        </FormControl>
                      </FormItem>
                    )}
                  />
                </div>
              </div>
              <Button type="submit" className="w-full" disabled={isLoading}>
                {isLoading ? (
                  <div className="flex items-center justify-center">
                    <span className="animate-spin rounded-full h-6 w-6 border-b-2 border-white mr-2"></span>
                    Creating Case...
                  </div>
                ) : (
                  <>
                    <Scale className="w-4 h-4 mr-2" />
                    Create Case
                  </>
                )}
              </Button>
            </form>
          </Form>
        </CardContent>
      </Card>
    </>
  );
};

// Additional components with proper TypeScript typing

interface CasePreviewProps {
  data: FormValues;
}

const CasePreview: React.FC<CasePreviewProps> = ({ data }) => {
  return (
    <div className="bg-secondary p-4 rounded-lg mt-8">
      <h3 className="text-lg font-semibold mb-4">Case Preview</h3>
      <div className="grid grid-cols-2 gap-4">
        <div>
          <p className="font-medium">Title:</p>
          <p>{data.title}</p>
        </div>
        <div>
          <p className="font-medium">Client:</p>
          <p>{data.clientName}</p>
        </div>
        <div>
          <p className="font-medium">Type:</p>
          <p>{caseTypes.find((type) => type.value === data.caseType)?.label}</p>
        </div>
        <div>
          <p className="font-medium">Incident Date:</p>
          <p>
            {data.incidentDate ? format(data.incidentDate, "PP") : "Not set"}
          </p>
        </div>
        <div>
          <p className="font-medium">Filing Date:</p>
          <p>{data.filingDate ? format(data.filingDate, "PP") : "Not set"}</p>
        </div>
        <div>
          <p className="font-medium">Estimated Duration:</p>
          <p>{data.estimatedDuration} months</p>
        </div>
        <div>
          <p className="font-medium">Case Value:</p>
          <p>${data.caseValue.toLocaleString()}</p>
        </div>
        <div>
          <p className="font-medium">Priority:</p>
          <Badge variant={data.priority > 3 ? "destructive" : "secondary"}>
            {data.priority}
          </Badge>
        </div>
        <div>
          <p className="font-medium">Confidential:</p>
          <p>{data.isConfidential ? "Yes" : "No"}</p>
        </div>
      </div>
    </div>
  );
};

interface ProgressBarProps {
  currentStep: number;
  totalSteps: number;
}

const ProgressBar: React.FC<ProgressBarProps> = ({
  currentStep,
  totalSteps,
}) => {
  return (
    <div className="w-full bg-secondary rounded-full h-2.5 mb-6">
      <div
        className="bg-primary h-2.5 rounded-full transition-all duration-500 ease-in-out"
        style={{ width: `${(currentStep / totalSteps) * 100}%` }}
      ></div>
    </div>
  );
};

export { CreateLegalCase as default, CasePreview, ProgressBar };
