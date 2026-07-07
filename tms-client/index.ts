import { Temporal } from "@js-temporal/polyfill";
import { Student, isStudent } from "./models/student.model";

function processStudent(raw: unknown) {
  if (isStudent(raw)) {
    const gpaDisplay = raw.gpa?.toFixed(2) ?? "Not yet graded";
    console.log(`Student ${raw.name} GPA: ${gpaDisplay}`);
  } else {
    console.error("Invalid student data received");
  }
}

processStudent({ id: "STU-001", name: "Hana", gpa: 3.7 });
processStudent(42);

import { parseStudent } from "./models/student.model";

console.log(parseStudent({ id: "STU-001", name: "Hana" }));

try {
  parseStudent({ id: 42, name: "Test" });
} catch (err) {
  console.error((err as Error).message);
}