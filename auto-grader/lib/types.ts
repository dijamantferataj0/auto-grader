export interface UploadResponse {
  teacherId: number;
  processedStudents: number;
  totalExams: number;
  summaries: ExamSummary[];
}

export interface ExamSummary {
  examId: number;
  studentId: number;
  totalTasks: number;
  correctTasks: number;
  scorePercentage: number;
}

export interface GradingResult {
  taskId: number;
  expression: string;
  isCorrect: boolean;
  calculatedValue: number;
  expectedValue: number;
}

export interface MathEvaluationRequest {
  expression: string;
}

export interface MathEvaluationResponse {
  result: number;
  isValid: boolean;
  errorMessage?: string;
}

export interface LoginRequest {
  userId: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  userId: string;
  role: string;
}
