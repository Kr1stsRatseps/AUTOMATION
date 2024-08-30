//HARBOUR DUES

double amt = 0.0;
double qty = 0.0;
double cost1 = 0.079; //25,000 - 45,000 per 12h
double cost2 = 0.763; //45,001 - 90,000 per 12h
double cost3 = 73.450; //above 90,000 per 12h
double GT = portValues["Length Over All"][0];
double time = 0.0;

for(int i= 0; i < portDates["Vessel undocked"].Count; i++){
  time += Math.Ceiling(((dates["Vessel undocked"][i]-dates["Docked [All Fast] at terminal"][i]).TotalHours)/12); 
}

if(GT > 250 && GT <= 45000){
  amt = cost1 * time * GT;
}else if(GT > 45000 && GT <= 90000){
  amt = cost2 * time * GT;
}else if(GT > 90000){
  amt = cost3 * time * GT;
}

if(amt > 0){
  qty = 1;
}

return new ResultValue(qty, amt, "USD");
