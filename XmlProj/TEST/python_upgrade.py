import xml.etree.ElementTree as ET
from datetime import datetime

# datetime function
def parse_datetime(datetime_str):
    try:
        return datetime.strptime(datetime_str, '%Y-%m-%dT%H:%M:%S.%f')
    except ValueError:
        return None

# path to the XML file
xml_file_path = 'xml_paraugs.xml'

namespace = {'ns': 'http://www.mscgva.ch/service/vci/2010/11/16'}

# Parse XML file
tree = ET.parse(xml_file_path)
root = tree.getroot()

# declering variables
length_over_all = None
port = None
date = None
husbandry_items = {}
husbandry_values = {}

# asaining right values
for item in root.findall('.//ns:item', namespace):
    tag = item.attrib.get('tag')
    value = item.attrib.get('value')
    type = item.attrib.get('type')
    index = item.attrib.get('index')
    
    if tag == 'Length Over All':
        if type == 'N':
            length_over_all = float(value)
        elif type == 'S':
            length_over_all = str(value)
        elif type == 'D':
            length_over_all = parse_datetime(value)
    
    elif tag == 'Vessel undocked':
        if type == 'N':
            date = float(value)
        elif type == 'S':
            date = str(value)
        elif type == 'D':
            date = parse_datetime(value)

    elif tag == 'Docked side to [Port / Starboard]':
        if type == 'N':
            port = float(value)
        elif type == 'S':
            port = str(value)
        elif type == 'D':
            port = parse_datetime(value)
    
    elif tag == 'Husbandry Item':
        if type == 'N':
            husbandry_items[int(index)] = float(value)
        elif type == 'S':
            husbandry_items[int(index)] = str(value)
        elif type == 'D':
            husbandry_items[int(index)] = parse_datetime(value)

    elif tag == 'Husbandry Value':
        if type == 'N':
            husbandry_values[int(index)] = float(value)
        elif type == 'S':
            husbandry_values[int(index)] = str(value)
        elif type == 'D':
            husbandry_values[int(index)] = parse_datetime(value)

# input OVA code here to calculate
amt = 0.0
qty = 0.0
cost = 0.88

for index, item in husbandry_items.items():
    if item == "Crew changes":
        amt += cost * husbandry_values[index]
        if amt > 0:
            qty = 1
# prints out values and results 
print("Qty:", qty, "Amt:", amt, "EUR")
'''print("LOA:", length_over_all)
print("dock:", port)
print("Date:", date)
print("HusItems:", husbandry_items)
print("HusValues:", husbandry_values)
'''

