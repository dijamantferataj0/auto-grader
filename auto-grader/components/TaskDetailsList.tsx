'use client';

import { GradingResult } from '@/lib/types';

interface TaskDetailsListProps {
  results: GradingResult[];
}

export default function TaskDetailsList({ results }: TaskDetailsListProps) {
  return (
    <div className="space-y-3">
      {results.map((result, index) => (
        <div
          key={index}
          className={`border-2 rounded-lg p-4 ${
            result.isCorrect
              ? 'border-green-200 bg-green-50'
              : 'border-red-200 bg-red-50'
          }`}
        >
          <div className="flex items-start justify-between">
            <div className="flex-1">
              <div className="flex items-center gap-2 mb-2">
                <span className="text-sm font-semibold text-slate-600">Task #{result.taskId}</span>
                <span className={`px-2 py-1 rounded-full text-xs font-bold ${
                  result.isCorrect
                    ? 'bg-green-600 text-white'
                    : 'bg-red-600 text-white'
                }`}>
                  {result.isCorrect ? '✓ Correct' : '✗ Incorrect'}
                </span>
              </div>

              <div className="font-mono text-lg mb-2">{result.expression}</div>

              <div className="grid grid-cols-2 gap-4 text-sm">
                <div>
                  <span className="font-semibold">Calculated:</span>{' '}
                  <span className="font-mono">{result.calculatedValue.toFixed(2)}</span>
                </div>
                <div>
                  <span className="font-semibold">Expected:</span>{' '}
                  <span className="font-mono">{result.expectedValue.toFixed(2)}</span>
                </div>
              </div>
            </div>

            <div className="text-3xl ml-4">
              {result.isCorrect ? '✅' : '❌'}
            </div>
          </div>
        </div>
      ))}
    </div>
  );
}
