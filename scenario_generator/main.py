"""
Usage:
  python main.py <func>
  python main.py generate_scenarios
  python main.py generate_execution_scripts
  python main.py generate_unit_tests
Options:
  -h --help     Show this screen.
  --version     Show version.
"""

__author__  = 'Chris Joakim'
__email__   = "chjoakim@microsoft.com,christopher.joakim@gmail.com"
__license__ = "MIT"
__version__ = "2020.11.07"

import json
import sys
import time
import os

import arrow
import jinja2

from docopt import docopt

def print_options(msg):
    print(msg)
    arguments = docopt(__doc__, version=__version__)
    print(arguments)

def provisioning_types():
    return 'standard,autoscale'.split(',')

def replication_types():
    return 'single,multi-region,multi-master'.split(',')

def availability_zone_types():
    return ['azone', 'noazone']

def database_gb_sizes():
    return '3,300,30000'.split(',')

def region_count(replication_type):
    if replication_type == 'multi-region':
        return 3
    elif replication_type == 'multi-master':
        return 3
    else:
        return 1

def generate_scenarios():
    seq, rc, script_lines = 0, 0, list()

    script_lines.append('#!/bin/bash')
    script_lines.append('')
    script_lines.append('# Execute the generated matrix of specifications.')
    script_lines.append('# Chris Joakim, Microsoft, {}'.format(current_date()))
    script_lines.append('')

    for pt in provisioning_types():
        for rt in replication_types():
            rc = region_count(rt)
            for az in availability_zone_types():
                for gb in database_gb_sizes():
                    seq = seq + 1
                    specfile   = 'specs/{}-{}-{}-{}-{}-{}gb.txt'.format(seq, pt, rt, rc, az, gb)
                    resultfile = '{}-{}-{}-{}-{}-{}gb.json'.format(seq, pt, rt, rc, az, gb)
                    content = "\n".join(specification_lines(seq, pt, rt, rc, az, gb))
                    write_file(specfile, content)
                    script_lines.append('dotnet run {} > {}'.format(specfile, resultfile))

    script_lines.append('')
    write_file('execute_specs_matrix.sh', "\n".join(script_lines))

def specification_lines(seq, pt, rt, rc, az, gb):
    az_bool = 'false'
    if az == 'azone':
        az_bool = 'true'

    lines = list()
    lines.append('Azure CosmosDB Cost Calculator Specification File')
    lines.append('version: {}'.format(current_date()))
    lines.append('')
    lines.append('container:            container{}'.format(seq))
    lines.append('provisioning_type:    {}'.format(pt))
    lines.append('replication_type:     {}'.format(rt))
    lines.append('region_count:         {}'.format(rc))
    lines.append('availability_zone:    {}'.format(az_bool))
    lines.append('size_in_gb:           {}'.format(gb))
    lines.append('synapse_link_enabled: {}'.format('true'))
    lines.append('calculate_costs:      {}'.format('true'))
    lines.append('')
    return lines

def generate_execution_scripts():
    print('generate_execution_scripts')

def generate_unit_tests():
    print('generate_unit_tests')

def current_date():
    utc = arrow.utcnow()
    return str(utc.to('US/Eastern')).split('T')[0]

def write_file(outfile, s, verbose=True):
    with open(outfile, 'w') as f:
        f.write(s)
        if verbose:
            print('file written: {}'.format(outfile))

def render(template, values):
    return template.render(values)

def get_template(root_dir, name):
    filename = 'templates/{}'.format(name)
    return cls.get_jinja2_env(root_dir).get_template(filename)

def get_jinja2_env(root_dir):
    print('get_jinja2_env root_dir: {}'.format(root_dir))
    return jinja2.Environment(
        loader = jinja2.FileSystemLoader(
            root_dir), autoescape=True)


if __name__ == "__main__":

    if len(sys.argv) > 1:
        func = sys.argv[1].lower()
        if func == 'generate_scenarios':
            generate_scenarios()
        elif func == 'generate_execution_scripts':
            generate_execution_scripts()
        elif func == 'generate_unit_tests':
            generate_unit_tests()
        else:
            print_options('Error: invalid function: {}'.format(func))
    else:
        print_options('Error: no command-line function given')
