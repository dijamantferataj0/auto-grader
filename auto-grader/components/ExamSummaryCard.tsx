'use client';

import Link from 'next/link';
import { ExamSummary } from '@/lib/types';

interface ExamSummaryCardProps {
  summary: ExamSummary;
}

export default function ExamSummaryCard({ summary }: ExamSummaryCardProps) {
  const getScoreColor = (percentage: number) => {
    if (percentage >= 80) return 'bg-green-500';
    if (percentage >= 50) return 'bg-yellow-500';
    return 'bg-red-500';
  };

  const getScoreBg = (percentage: number) => {
    if (percentage >= 80) return 'bg-green-50 border-green-200';
    if (percentage >= 50) return 'bg-yellow-50 border-yellow-200';
    return 'bg-red-50 border-red-200';
  };

  return (
    <div className={`border-2 rounded-lg p-6 shadow-md ${getScoreBg(summary.scorePercentage)}`}>
      <div className="flex justify-between items-start mb-4">
        <div>
          <h3 className="text-lg font-bold">Exam #{summary.examId}</h3>
          <p className="text-sm text-slate-600">Student ID: {summary.studentId}</p>
        </div>
        <div className="text-right">
          <div className="text-3xl font-bold">{summary.scorePercentage.toFixed(1)}%</div>
          <div className="text-sm text-slate-600">Score</div>
        </div>
      </div>

      <div className="w-full bg-slate-200 rounded-full h-3 mb-4">
        <div
          className={`h-3 rounded-full transition-all ${getScoreColor(summary.scorePercentage)}`}
          style={{ width: `${summary.scorePercentage}%` }}
        ></div>
      </div>

      <div className="flex justify-between text-sm mb-4">
        <div>
          <span className="font-semibold">Correct:</span> {summary.correctTasks} / {summary.totalTasks}
        </div>
        <div>
          <span className="font-semibold">Incorrect:</span> {summary.totalTasks - summary.correctTasks}
        </div>
      </div>

      <Link
        href={`/analytics/${summary.examId}`}
        className="block w-full text-center bg-blue-600 text-white py-2 rounded-lg font-semibold hover:bg-blue-700 transition-colors"
      >
        View Details
      </Link>
    </div>
  );
}
