"""
Usage:
  python main.py <func>
  python main.py generate_matrix_specs
  python main.py generate_unit_tests
Options:
  -h --help     Show this screen.
  --version     Show version.
"""

__author__  = 'Chris Joakim'
__email__   = "chjoakim@microsoft.com,christopher.joakim@gmail.com"
__license__ = "MIT"
__version__ = "2020.11.08"

import json
import os
import sys
import time

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

def generate_matrix_specs():
    seq, rc, script_lines, markdown_lines = 0, 0, list(), list()

    script_lines.append('#!/bin/bash')
    script_lines.append('')
    script_lines.append('# Execute the generated matrix of specifications.')
    script_lines.append('# Chris Joakim, Microsoft, {}'.format(current_date()))
    script_lines.append('')

    markdown_lines.append('| Prov | Repl | RC | AvZ | DB GB | Spec File |')
    markdown_lines.append('| ---- | ---- | -- | --- | ----- | --------- |')

    for pt in provisioning_types():
        for rt in replication_types():
            rc = region_count(rt)
            for az in availability_zone_types():
                for gb in database_gb_sizes():
                    valid_combo = True
                    if (rt == 'multi-region') and (az == 'azone'):
                        # see https://docs.microsoft.com/en-us/azure/cosmos-db/high-availability#availability-zone-support
                        valid_combo = False 
                    if valid_combo:
                        seq = seq + 1
                        specbase   = '{}-{}-{}-{}-{}-{}gb.txt'.format(seq, pt, rt, rc, az, gb)
                        specfile   = 'spec_matrix/{}'.format(specbase)
                        resultfile = 'spec_matrix/out/{}-{}-{}-{}-{}-{}gb.json'.format(seq, pt, rt, rc, az, gb)
                        content = "\n".join(specification_lines(seq, pt, rt, rc, az, gb))
                        write_file(specfile, content)
                        script_lines.append('dotnet run {} > {}'.format(specfile, resultfile))
                        markdown_lines.append('| {} | {} | {} | {} | {} | {} |'.format(pt, rt, rc, az, gb, specbase))

    script_lines.append('')
    write_file('execute_spec_matrix.sh', "\n".join(script_lines))
    write_file('spec_matrix.md', "\n".join(markdown_lines))

def specification_lines(seq, pt, rt, rc, az, gb):
    az_bool = 'false'
    if az == 'azone':
        az_bool = 'true'
    if rc < 2:
        repl_gb = 0.0
    else:
        repl_gb = float(gb) / 10.0

    lines = list()
    lines.append('Azure CosmosDB Cost Calculator Specification File')
    lines.append('')
    lines.append('container:               container{}'.format(seq))
    lines.append('provisioning_type:       {}'.format(pt))
    lines.append('replication_type:        {}'.format(rt))
    lines.append('region_count:            {}'.format(rc))
    lines.append('availability_zone:       {}'.format(az_bool))
    lines.append('size_in_gb:              {}'.format(gb))
    lines.append('replicated_gb_per_month: {}'.format(repl_gb))
    lines.append('synapse_link_enabled:    {}'.format('true'))
    lines.append('calculate_costs:         {}'.format('true'))
    lines.append('')
    return lines

def generate_unit_tests():
    print('generate_unit_tests')
    seq = 0
    repo_home = calculator_home_dir()
    spec_dir  = '{}/cosmos_calc/spec_matrix'.format(repo_home)
    xunit_dir = '{}/cosmos_calc.tests'.format(repo_home)

    for pt in provisioning_types():
        for rt in replication_types():
            rc = region_count(rt)
            for az in availability_zone_types():
                for gb in database_gb_sizes():
                    valid_combo = True
                    if (rt == 'multi-region') and (az == 'azone'):
                        # see https://docs.microsoft.com/en-us/azure/cosmos-db/high-availability#availability-zone-support
                        valid_combo = False 
                    if valid_combo:
                        seq = seq + 1
                        specbase   = '{}-{}-{}-{}-{}-{}gb.txt'.format(seq, pt, rt, rc, az, gb)
                        specfile   = '{}/{}'.format(spec_dir, specbase)
                        resultfile = '{}/out/{}-{}-{}-{}-{}-{}gb.json'.format(spec_dir, seq, pt, rt, rc, az, gb)
                        classname  = 'SpecMatrix{}Test'.format(seq)
                        xunitfile  = '{}/{}.cs'.format(xunit_dir, classname, seq)
                        generate_unit_test(seq, pt, rt, rc, az, gb, specfile, resultfile, classname, xunitfile)   

def generate_unit_test(seq, pt, rt, rc, az, gb, specfile, resultfile, classname, xunitfile):
    print('generate_unit_test')
    print('  specfile:   {}'.format(specfile))
    print('  resultfile: {}'.format(resultfile))                
    print('  xunitfile:  {}'.format(xunitfile)) 

    speclines = read_lines(specfile)
    resultobj = read_json(resultfile)

    values = dict()
    values['date'] = current_date()
    values['classname'] = classname
    values['seq'] = seq
    values['pt'] = pt
    values['rt'] = rt
    values['rc'] = rc
    if az == 'azone':
        values['azbool'] = 'true'
    else:
        values['azbool'] = 'false'
    values['gb'] = gb
    values['ru'] = 333

    for key in resultobj.keys():
        values[key] = resultobj[key]
    values['spec'] = "".join(speclines)
    values['calc'] = json.dumps(resultobj, sort_keys=False, indent=2) 
    t = get_template('XunitTest.txt')
    code = t.render(values)
    print(code)

def current_date():
    utc = arrow.utcnow()
    return str(utc.to('US/Eastern')).split('T')[0]

def calculator_home_dir():
    return os.environ['AZURE_COSMOSDB_COST_CALCULATOR_HOME']

def read_lines(infile):
    lines = list()
    with open(infile, 'rt') as f:
        for line in f:
            lines.append(line)
    return lines

def read_json(infile):
    with open(infile, 'rt') as f:
        return json.loads(f.read())

def write_file(outfile, s, verbose=True):
    with open(outfile, 'w') as f:
        f.write(s)
        if verbose:
            print('file written: {}'.format(outfile))

def render(template, values):
    return template.render(values)

def get_template(tname):
    repo_home = calculator_home_dir()
    templates_dir = '{}/spec_generator/templates'.format(repo_home)
    return get_jinja2_env(templates_dir).get_template(tname)

def get_jinja2_env(templates_dir):
    print('get_jinja2_env root_dir: {}'.format(templates_dir))
    return jinja2.Environment(
        loader = jinja2.FileSystemLoader(
            templates_dir), autoescape=True)


if __name__ == "__main__":

    if len(sys.argv) > 1:
        func = sys.argv[1].lower()
        if func == 'generate_matrix_specs':
            generate_matrix_specs()
        elif func == 'generate_unit_tests':
            generate_unit_tests()
        else:
            print_options('Error: invalid function: {}'.format(func))
    else:
        print_options('Error: no command-line function given')
