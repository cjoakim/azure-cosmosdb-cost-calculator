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
    return [False, True]

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
    seq, rc = 0, 0
    for pt in provisioning_types():
        for rt in replication_types():
            rc = region_count(rt)
            for az_bool in availability_zone_types():
                for gb in database_gb_sizes():
                    seq = seq + 1
                    print('{}: pt: {}, rt: {}, az: {}, gb: {}, rc: {}'.format(
                        seq, pt, rt, az_bool, gb, rc))

def generate_execution_scripts():
    print('generate_execution_scripts')

def generate_unit_tests():
    print('generate_unit_tests')


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
