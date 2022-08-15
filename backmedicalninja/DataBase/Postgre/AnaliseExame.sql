SELECT * FROM public.series
where study_fk = 155


SELECT * FROM public.study
where pk > 156

SELECT * FROM public.study
where patient_fk = 46

select study.pk, study.created_time, patient.pk, patient_id.pk,
patient_id.pat_id, patient_id.issuer_fk,
person_name.family_name, person_name.given_name, person_name.middle_name,
issuer.entity_id
from study, patient, patient_id, person_name, issuer
where study.pk = 156 
and study.patient_fk = patient.pk
and patient_id.pk = patient.patient_id_fk
and patient.pat_name_fk = person_name.pk
and issuer.pk = patient_id.issuer_fk